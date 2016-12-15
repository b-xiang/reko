#region License
/* 
 * Copyright (C) 1999-2016 John K�ll�n.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using Reko.Evaluation;
using Reko.Core;
using Reko.Core.Code;
using Reko.Core.Expressions;
using Reko.Core.Operators;
using Reko.Core.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace Reko.Analysis
{
    /// <summary>
    /// Performs propagation by replacing occurences of expressions with 
    /// simpler expressions if these are beneficial. Constants are folded,
    /// and so on.
    /// </summary>
    /// <remarks>
    /// This is a useful transform that doesn't cause too many problems for
    /// later transforms. Calling it will flush out lots of dead expressions
    /// that can be removed with DeadCode.Eliminate()
    /// </remarks>
    public class ValuePropagator : InstructionVisitor<Instruction>
    {
        private static TraceSwitch trace = new TraceSwitch("ValuePropagation", "Traces value propagation");

        private IProcessorArchitecture arch;
        private SsaState ssa;
        private ExpressionSimplifier eval;
        private SsaEvaluationContext evalCtx;
        private SsaIdentifierTransformer ssaIdTransformer;

        public ValuePropagator(IProcessorArchitecture arch, SsaState ssa)
        {
            this.arch = arch;
            this.ssa = ssa;
            this.evalCtx = new SsaEvaluationContext(arch, ssa.Identifiers);
            this.eval = new ExpressionSimplifier(evalCtx);
            this.ssaIdTransformer = new SsaIdentifierTransformer(ssa);
        }

        public bool Changed { get { return eval.Changed; } set { eval.Changed = value; } }

        public void Transform()
        {
            do
            {
                Changed = false;
                foreach (Statement stm in ssa.Procedure.Statements)
                {
                    Transform(stm);
                }
            } while (Changed);
        }

        public void Transform(Statement stm)
        {
            evalCtx.Statement = stm;
            try
            {
                if (trace.TraceVerbose) Debug.WriteLine(string.Format("From: {0}", stm.Instruction.ToString()));
                stm.Instruction = stm.Instruction.Accept(this);
                if (trace.TraceVerbose) Debug.WriteLine(string.Format("  To: {0}", stm.Instruction.ToString()));
            } catch (Exception ex)
            {
                throw new StatementCorrelatedException(
                    stm, 
                    string.Format("An error occurred while processing the statement {0}.", stm),
                    ex);
            }
        }

        #region InstructionVisitor<Instruction> Members

        public Instruction VisitAssignment(Assignment a)
        {
            a.Src = a.Src.Accept(eval);
            ssa.Identifiers[a.Dst].DefExpression = a.Src;
            return a;
        }

        public Instruction VisitBranch(Branch b)
        {
            b.Condition = b.Condition.Accept(eval);
            return b;
        }

        public Instruction VisitCallInstruction(CallInstruction ci)
        {
            ci.Callee = ci.Callee.Accept(eval);
            var pc = ci.Callee as ProcedureConstant;
            if (pc != null && pc.Procedure.Signature != null && pc.Procedure.Signature.ParametersValid)
            {
                var binder = new CallInstructionBinder(ci);
                var ab = arch.CreateFrameApplicationBuilder(
                      binder,
                      ci.CallSite,
                      ci.Callee);
                evalCtx.Statement.Instruction = ab.CreateInstruction(pc.Procedure.Signature, pc.Procedure.Characteristics);
                ssaIdTransformer.Transform(evalCtx.Statement, ci);
                return evalCtx.Statement.Instruction;
            }
            foreach (var use in ci.Uses)
            {
                use.Expression = use.Expression.Accept(eval);
            }
            foreach (var def in ci.Definitions
                .Where(d => !( d.Expression is Identifier)))
            {
                def.Expression = def.Expression.Accept(eval);
            }
            return ci;
        }


        public Instruction VisitDeclaration(Declaration decl)
        {
            if (decl.Expression != null)
                decl.Expression = decl.Expression.Accept(eval);
            return decl;
        }

        public Instruction VisitDefInstruction(DefInstruction def)
        {
            return def;
        }

        public Instruction VisitGotoInstruction(GotoInstruction gotoInstruction)
        {
            return gotoInstruction;
        }

        public Instruction VisitPhiAssignment(PhiAssignment phi)
        {
            var src = phi.Src.Accept(eval);
            PhiFunction f = src as PhiFunction;
            if (f != null)
                return new PhiAssignment(phi.Dst, f);
            else
                return new Assignment(phi.Dst, src);
        }

        public Instruction VisitReturnInstruction(ReturnInstruction ret)
        {
            if (ret.Expression != null)
                ret.Expression = ret.Expression.Accept(eval);
            return ret;
        }

        public Instruction VisitSideEffect(SideEffect side)
        {
            side.Expression = side.Expression.Accept(eval);
            return side;
        }

        public Instruction VisitStore(Store store)
        {
            store.Src = store.Src.Accept(eval);
            var idDst = store.Dst as Identifier;
            if (idDst == null || (!(idDst.Storage is OutArgumentStorage)))
            {
                store.Dst = store.Dst.Accept(eval);
            }
            return store;
        }

        public Instruction VisitSwitchInstruction(SwitchInstruction si)
        {
            si.Expression = si.Expression.Accept(eval);
            return si;
        }

        public Instruction VisitUseInstruction(UseInstruction u)
        {
            return u;
        }

        #endregion

        private class CallInstructionBinder : IStorageBinder
        {
            private CallInstruction ci;

            public CallInstructionBinder(CallInstruction ci)
            {
                this.ci = ci;
            }

            public Identifier EnsureFlagGroup(FlagRegister flagRegister, uint flagGroupBits, string name, DataType dataType)
            {
                throw new NotImplementedException();
            }

            public Identifier EnsureFpuStackVariable(int v, DataType dataType)
            {
                throw new NotImplementedException();
            }

            public Identifier EnsureIdentifier(Storage stg)
            {
                // If we can't find the storage, we're in trouble.
                // it could mean that no statement in this fn defined it,
                // so we probably should create it as a live-in identifier.
                return ci.Uses
                    .Select(u => u.Expression)
                    .OfType<Identifier>()
                    .Where(i => i.Storage == stg)
                    .First();
            }

            public Identifier EnsureRegister(RegisterStorage reg)
            {
                return ci.Uses
                   .Select(u => u.Expression)
                   .OfType<Identifier>()
                   .Where(i => i.Storage.Name == reg.Name)
                   .First();
            }

            public Identifier EnsureSequence(Storage head, Storage tail, DataType dt)
            {
                throw new NotImplementedException();
            }

            public Identifier EnsureStackVariable(int v, DataType dataType)
            {
                throw new NotImplementedException();
            }
        }

    }
}
