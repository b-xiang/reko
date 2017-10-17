﻿#region License
/* 
 * Copyright (C) 1999-2017 John Källén.
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

using Reko.Core.Machine;
using Reko.Core.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reko.Arch.Msp430
{
    public class Msp430Instruction : MachineInstruction
    {
        private static Dictionary<Opcode, InstructionClass> classes = new Dictionary<Opcode, InstructionClass>
        {
            { Opcode.call, InstructionClass.Call | InstructionClass.Transfer },
            { Opcode.jc, InstructionClass.Conditional | InstructionClass.Transfer },
            { Opcode.jge, InstructionClass.Conditional | InstructionClass.Transfer },
            { Opcode.jl, InstructionClass.Conditional | InstructionClass.Transfer },
            { Opcode.jmp, InstructionClass.Transfer },
            { Opcode.jn, InstructionClass.Conditional | InstructionClass.Transfer },
            { Opcode.jnc, InstructionClass.Conditional | InstructionClass.Transfer },
            { Opcode.jnz, InstructionClass.Conditional | InstructionClass.Transfer },
            { Opcode.jz, InstructionClass.Conditional | InstructionClass.Transfer },
        };

        public Opcode opcode;
        public PrimitiveType dataWidth;
        public MachineOperand op1;
        public MachineOperand op2;

        public override InstructionClass InstructionClass
        {
            get
            {
                InstructionClass c;
                if (!classes.TryGetValue(opcode, out c))
                    return InstructionClass.Linear;
                return c;
            }
        }

        public override bool IsValid
        {
            get { return opcode != Opcode.invalid; }
        }

        public override int OpcodeAsInteger
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MachineOperand GetOperand(int i)
        {
            throw new NotImplementedException();
        }

        public override void Render(MachineInstructionWriter writer, MachineInstructionWriterOptions options)
        {
            var sb = new StringBuilder(opcode.ToString());
            if (dataWidth != null)
            {
                sb.AppendFormat(".{0}", dataWidth.BitSize == 8 
                    ? "b" 
                    : dataWidth.BitSize == 16
                        ? "w"
                        : "a" );
            }
            writer.WriteOpcode(sb.ToString());
            if (op1 != null)
            {
                writer.Tab();
                Write(op1, writer, options);
                if (op2 != null)
                {
                    writer.Write(",");
                    Write(op2, writer, options);
                }
            }
        }

        private void Write(MachineOperand op, MachineInstructionWriter writer, MachineInstructionWriterOptions options)
        {
            if (op is AddressOperand && (InstructionClass & InstructionClass.Transfer) == 0)
            {
                writer.Write("&");
            }
            if (op is ImmediateOperand && opcode != Opcode.call)
            {
                writer.Write("#");
            }
            op.Write(writer, options);
        }
    }
}