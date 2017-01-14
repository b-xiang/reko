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

using System;
using System.Collections.Generic;
using Reko.Core;
using Reko.Core.Machine;
using Reko.Core.Types;
using Reko.Core.Expressions;
using System.Diagnostics;

namespace Reko.Arch.Avr
{
    // Opcode map: http://lyons42.com/AVR/Opcodes/AVRAllOpcodes.html
    // Opcode map: https://en.wikipedia.org/wiki/Atmel_AVR_instruction_set
    public class Avr8Disassembler : DisassemblerBase<AvrInstruction>
    {
        private static OpRec[] oprecs;

        private Avr8Architecture arch;
        private Address addr;
        private ImageReader rdr;

        public Avr8Disassembler(Avr8Architecture arch, ImageReader rdr)
        {
            this.arch = arch;
            this.rdr = rdr;
        }

        public override AvrInstruction DisassembleInstruction()
        {
            ushort wInstr;
            this.addr = rdr.Address;
            if (!rdr.TryReadUInt16(out wInstr))
                return null;
            var instr = oprecs[wInstr >> 12].Decode(this, wInstr);
            instr.Address = addr;
            var length = rdr.Address - addr;
            instr.Length = (int)length;
            return instr;
        }

        public AvrInstruction Decode(ushort wInstr, Opcode opcode, string fmt)
        {
            var ops = new List<MachineOperand>();
            int offset;
            ushort w2;
            for (int i = 0; i < fmt.Length; ++i)
            {
                MachineOperand op;
                switch (fmt[i++])
                {
                case ',':
                    continue;
                case '+':
                    op = IncDec(true, fmt[i++]);
                    break;
                case '-':
                    op = IncDec(false, fmt[i++]);
                    break;
                case 'A': // I/O location
                    op = ImmediateOperand.Byte((byte)(((wInstr >> 5) & 0x30) | (wInstr & 0xF)));
                    break;
                case 'B': // 8-bit immediate at bits 8..11 and 0..3
                    op = ImmediateOperand.Byte((byte)(((wInstr >> 4) & 0xF0) | (wInstr & 0x0F)));
                    break;
                case 'g': // 4-bit field in sgis
                    op = ImmediateOperand.Byte((byte)((wInstr >> 3) & 0x0F));
                    break;
                case 'h': // 3-bit field in sgis, indicating bit nr.
                    op = ImmediateOperand.Byte((byte)(wInstr & 7));
                    break;
                case 'I': // 4-bit immediate at bit 0
                    op = ImmediateOperand.Byte((byte)(wInstr & 0x0F));
                    break;
                case 'i': // 4-bit immediate at bit 4
                    op = ImmediateOperand.Byte((byte)((wInstr >> 4) & 0x0F));
                    break;
                case 'J': // Relative jump
                    offset = (short)((wInstr & 0xFFF) << 4);
                    offset = offset >> 3;
                    op = AddressOperand.Create(this.addr + 2 + offset);
                    break;
                case 'D': // Destination register
                    op = Register((wInstr >> 4) & 0x1F);
                    break;
                case 'd': // Destination register (r16-r31)
                    op = Register(0x10 | (wInstr >> 4) & 0x1F);
                    break;
                case 'R': // source register (5 bits)
                    op = Register((wInstr >> 5) & 0x10 | (wInstr) & 0x0F);
                    break;
                case 'r': // source register (4 bits)
                    op = Register((wInstr >> 4) & 0x10 | (wInstr >> 4) & 0x0F);
                    break;
                case 'K':
                    op = ImmediateOperand.Byte((byte)(((wInstr >> 4) & 0xF0) | (wInstr & 0xF)));
                    break;
                case 'P': // register pair source
                    op = Register((wInstr << 1) & ~1);
                    break;
                case 'p': // register pair destination
                    op = Register((wInstr >> 3) & ~1);
                    break;
                case 'Q':   // absolute address used by jump and call.
                    if (!rdr.TryReadLeUInt16(out w2))
                        return null;
                    op = AddressOperand.Ptr32(
                        (uint)(((wInstr >> 4) & 0x1F) << 18) |
                        (uint)((wInstr & 1) << 17) |
                        (uint)(w2 << 1));
                    break;
                case 'q':   // register pair used by adiw
                    op = Register(24 + ((wInstr >> 3) & 6));
                    break;
                case 's':   // immediate used by adiw/sbiw
                    op = ImmediateOperand.Byte((byte)(((wInstr >> 2) & 0x30) | (wInstr & 0xF)));
                    break;
                case 'w': // Trailing 16-bit absolute address
                    if (!rdr.TryReadLeUInt16(out w2))
                        return null;
                    op = AddressOperand.Ptr16(w2);
                    break;
                case 'o':   // Branch offset
                    offset = (short)wInstr;
                    offset = (short)(offset << 6);
                    offset = (short)(offset >> 8);
                    offset = (short)(offset & ~1);
                    op = AddressOperand.Create(this.addr + offset);
                    break;
                case 'X':
                    op = MemD(arch.x, 0);
                    break;
                case 'Y':
                    op = MemD(arch.y, 0);
                    break;
                case 'y':
                    op = MemD(arch.y, Displacement(wInstr));
                    break;
                case 'Z':
                    op = MemD(arch.z, 0);
                    break;
                case 'z':
                    op = MemD(arch.z, Displacement(wInstr));
                    break;
                default:
                    throw new NotImplementedException(string.Format("Unimplemented AVR8 format symbol '{0}'.'", fmt[i - 1]));
                }
                ops.Add(op);
            }
            return new AvrInstruction
            {
                opcode = opcode,
                operands = ops.ToArray(),
            };
        }

        private MachineOperand Register(int v)
        {
            return new RegisterOperand(arch.GetRegister(v & 0x1F));
        }

        private MachineOperand MemD(RegisterStorage baseReg, int displacement)
        {
            return new MemoryOperand(PrimitiveType.Byte)
            {
                Base = baseReg,
                Displacement = displacement
            };
        }

        private MachineOperand IncDec(bool inc, char cReg)
        {
            RegisterStorage reg;
            switch (cReg)
            {
            case 'X': reg = arch.x; break;
            case 'Y': reg = arch.y; break;
            case 'Z': reg = arch.z; break;
            default: Debug.Assert(false, "Must be X,y, or Z");
                reg = null;
                break;
            }
            return new MemoryOperand(PrimitiveType.Byte)
            {
                Base = reg,
                PostIncrement = inc,
                PreDecrement = !inc,
            };
        }

        private int Displacement(ushort wInstr)
        {
            return
                ((wInstr >> 8) & 0x20)
                | ((wInstr >> 6) & 0x18)
                | (wInstr & 7);
        }

        static Avr8Disassembler()
        {
            var oprecs0 = new OpRec[]
            {
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.movw, "p,P"),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.cpc, "D,R"),
                new BOpRec(Opcode.cpc, "D,R"),
                new BOpRec(Opcode.cpc, "D,R"),
                new BOpRec(Opcode.cpc, "D,R"),

                new BOpRec(Opcode.sbc, "D,R"),
                new BOpRec(Opcode.sbc, "D,R"),
                new BOpRec(Opcode.sbc, "D,R"),
                new BOpRec(Opcode.sbc, "D,R"),

                new BOpRec(Opcode.add, "D,R"),
                new BOpRec(Opcode.add, "D,R"),
                new BOpRec(Opcode.add, "D,R"),
                new BOpRec(Opcode.add, "D,R"),
            };

            var oprecs1 = new OpRec[]
            {
                new BOpRec(Opcode.cpse, "D,R"),
                new BOpRec(Opcode.cp,   "D,R"),
                new BOpRec(Opcode.sub,  "D,R"),
                new BOpRec(Opcode.adc,  "D,R"),
            };

            var oprecs2 = new OpRec[]
            {
                new BOpRec(Opcode.and, "D,R"),
                new BOpRec(Opcode.eor, "D,R"),
                new BOpRec(Opcode.or, "D,R"),
                new BOpRec(Opcode.mov, "R,r"),
            };

            var oprecs80 = new OpRec[]
            {
                new BOpRec(Opcode.ld,  "D,X"),
                new BOpRec(Opcode.ldd, "D,z"),
                new BOpRec(Opcode.ldd, "D,z"),
                new BOpRec(Opcode.ldd, "D,z"),

                new BOpRec(Opcode.ldd, "D,z"),
                new BOpRec(Opcode.ldd, "D,z"),
                new BOpRec(Opcode.ldd, "D,z"),
                new BOpRec(Opcode.ldd, "D,z"),

                new BOpRec(Opcode.ld,  "D,y"),
                new BOpRec(Opcode.ldd, "D,y"),
                new BOpRec(Opcode.ldd, "D,y"),
                new BOpRec(Opcode.ldd, "D,y"),

                new BOpRec(Opcode.ldd, "D,y"),
                new BOpRec(Opcode.ldd, "D,y"),
                new BOpRec(Opcode.ldd, "D,y"),
                new BOpRec(Opcode.ldd, "D,y"),
            };

            var oprecs82 = new OpRec[]
            {
                new BOpRec(Opcode.st, "Z,D"),
                new BOpRec(Opcode.std, "z,D"),
                new BOpRec(Opcode.std, "z,D"),
                new BOpRec(Opcode.std, "z,D"),

                new BOpRec(Opcode.std, "z,D"),
                new BOpRec(Opcode.std, "z,D"),
                new BOpRec(Opcode.std, "z,D"),
                new BOpRec(Opcode.std, "z,D"),

                new BOpRec(Opcode.st, "y,D"),
                new BOpRec(Opcode.std, "y,D"),
                new BOpRec(Opcode.std, "y,D"),
                new BOpRec(Opcode.std, "y,D"),

                new BOpRec(Opcode.std, "y,D"),
                new BOpRec(Opcode.std, "y,D"),
                new BOpRec(Opcode.std, "y,D"),
                new BOpRec(Opcode.std, "y,D"),
            };

            var oprecs8 = new OpRec[]
            {
                new GrpOpRec(0, 4, oprecs80),
                new GrpOpRec(0, 4, oprecs82),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
            };

            var oprecs94_8 = new OpRec[]
            {
                new BOpRec(Opcode.sec, ""),
                new BOpRec(Opcode.sez, ""),
                new BOpRec(Opcode.sen, ""),
                new BOpRec(Opcode.sev, ""),

                new BOpRec(Opcode.ses, ""),
                new BOpRec(Opcode.seh, ""),
                new BOpRec(Opcode.set, ""),
                new BOpRec(Opcode.sei, ""),

                new BOpRec(Opcode.clc, ""),
                new BOpRec(Opcode.clz, ""),
                new BOpRec(Opcode.cln, ""),
                new BOpRec(Opcode.clv, ""),

                new BOpRec(Opcode.cls, ""),
                new BOpRec(Opcode.clh, ""),
                new BOpRec(Opcode.clt, ""),
                new BOpRec(Opcode.cli, ""),

                new BOpRec(Opcode.ret, ""),
                new BOpRec(Opcode.reti, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.sleep, ""),
                new BOpRec(Opcode.@break, ""),
                new BOpRec(Opcode.wdr, ""),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.lpm, ""),
                new BOpRec(Opcode.elpm, ""),
                new BOpRec(Opcode.spm, ""),
                new BOpRec(Opcode.spm, ""),
            };

            var oprecs94_9 = new Dictionary<int, OpRec>
            {
                { 0, new BOpRec(Opcode.ijmp, "") },
                { 1, new BOpRec(Opcode.eijmp, "") },
                { 16, new BOpRec(Opcode.icall, "") },
                { 17, new BOpRec(Opcode.eicall, "") },
            };

            var oprecs90 = new OpRec[]
            {
                new BOpRec(Opcode.lds, "D,w"),
                new BOpRec(Opcode.ld, "D,+Z"),
                new BOpRec(Opcode.ld, "D,-Z"),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.lpm, "D,Z"),
                new BOpRec(Opcode.lpm, "D,+Z"),
                new BOpRec(Opcode.elpm, "D,Z"),
                new BOpRec(Opcode.elpm, "D,+Z"),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.ld, "D,+y"),
                new BOpRec(Opcode.ld, "D,-y"),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.ld, "D,X"),
                new BOpRec(Opcode.ld, "D,+X"),
                new BOpRec(Opcode.ld, "D,-X"),
                new BOpRec(Opcode.pop, "D"),
            };

            var oprecs92 = new OpRec[]
            {
                new BOpRec(Opcode.sts, "w,D"),
                new BOpRec(Opcode.st, "+Z,D"),
                new BOpRec(Opcode.st, "-Z,D"),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.st, "+y,D"),
                new BOpRec(Opcode.st, "-y,D"),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.st, "X,D"),
                new BOpRec(Opcode.st, "+X,D"),
                new BOpRec(Opcode.st, "-X,D"),
                new BOpRec(Opcode.push, "D"),
            };

            var oprecs94 = new OpRec[]
            {
                new BOpRec(Opcode.com, "D"),
                new BOpRec(Opcode.neg, "D"),
                new BOpRec(Opcode.swap, "D"),
                new BOpRec(Opcode.inc, "D"),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.asr, "D"),
                new BOpRec(Opcode.lsr, "D"),
                new BOpRec(Opcode.ror, "D"),

                new GrpOpRec(4, 5, oprecs94_8),
                new SparseOpRec(4, 5, oprecs94_9),
                new BOpRec(Opcode.dec, "D"),
                new BOpRec(Opcode.des, "i"),

                new BOpRec(Opcode.jmp, "Q"),
                new BOpRec(Opcode.jmp, "Q"),
                new BOpRec(Opcode.call, "Q"),
                new BOpRec(Opcode.call, "Q"),
            };

            var oprecs9 = new OpRec[]
            {
                new GrpOpRec(0, 4, oprecs90),
                new GrpOpRec(0, 4, oprecs90),
                new GrpOpRec(0, 4, oprecs92),
                new GrpOpRec(0, 4, oprecs92),

                new GrpOpRec(0, 4, oprecs94),
                new GrpOpRec(0, 4, oprecs94),   //$TODO: may need a oprecs95 for all the invalid "des"
                new BOpRec(Opcode.adiw, "q,s"),
                new BOpRec(Opcode.sbiw, "q,s"),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.sbis, "g,h"),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
            };

            var oprecsB = new OpRec[]
            {
                new BOpRec(Opcode.@in, "D,A"),
                new BOpRec(Opcode.@out, "A,D"),
            };

            var oprecsF = new OpRec[]
            {
                new CondOpRec(),
                new CondOpRec(),
                new CondOpRec(),
                new CondOpRec(),

                new CondOpRec(),
                new CondOpRec(),
                new CondOpRec(),
                new CondOpRec(),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),

                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
                new BOpRec(Opcode.invalid, ""),
            };

            oprecs = new OpRec[]
            {
                new GrpOpRec(8, 4, oprecs0),
                new GrpOpRec(10, 2, oprecs1),
                new GrpOpRec(10, 2, oprecs2),
                new BOpRec(Opcode.cpi, "d,B"),

                new BOpRec(Opcode.sbci, "d,B"),
                new BOpRec(Opcode.subi, "d,B"),
                new BOpRec(Opcode.ori, "d,B"),
                new BOpRec(Opcode.andi, "d,B"),

                new GrpOpRec(9, 3, oprecs8),
                new GrpOpRec(8, 4, oprecs9),
                new BOpRec(Opcode.invalid, ""),
                new GrpOpRec(0xB, 1, oprecsB),

                new BOpRec(Opcode.rjmp, "J"),
                new BOpRec(Opcode.rcall, "J"),
                new BOpRec(Opcode.ldi, "d,K"),
                new GrpOpRec(8, 4, oprecsF),
            };
        }

        public abstract class OpRec
        {
            public abstract AvrInstruction Decode(Avr8Disassembler dasm, ushort wInstr);
        }

        public class BOpRec : OpRec
        {
            private Opcode opcode;
            private string fmt;

            public BOpRec(Opcode opcode, string fmt)
            {
                this.opcode = opcode;
                this.fmt = fmt;
            }

            public override AvrInstruction Decode(Avr8Disassembler dasm, ushort wInstr)
            {
                return dasm.Decode(wInstr, opcode, fmt);
            }
        }

        public class GrpOpRec : OpRec
        {
            private int shift;
            private int mask;
            private OpRec[] oprecs;

            public GrpOpRec(int shift, int length, OpRec[] oprecs)
            {
                this.shift = shift;
                this.mask = (1 << length) - 1;
                this.oprecs = oprecs;
            }

            public override AvrInstruction Decode(Avr8Disassembler dasm, ushort wInstr)
            {
                int slot = (wInstr >> shift) & mask;
                return oprecs[slot].Decode(dasm, wInstr);
            }
        }

        public class SparseOpRec : OpRec
        {
            private int shift;
            private int mask;
            private Dictionary<int, OpRec> oprecs;

            public SparseOpRec(int shift, int length, Dictionary<int, OpRec> oprecs)
            {
                this.shift = shift;
                this.mask = (1 << length) - 1;
                this.oprecs = oprecs;
            }

            public override AvrInstruction Decode(Avr8Disassembler dasm, ushort wInstr)
            {
                int slot = (wInstr >> shift) & mask;
                OpRec oprec;
                if (!oprecs.TryGetValue(slot, out oprec))
                {
                    return dasm.Decode(wInstr, Opcode.invalid, "");
                }
                return oprec.Decode(dasm, wInstr);
            }
        }

        public class CondOpRec : OpRec
        {
            static Opcode[] branches = new Opcode[]
            {
                Opcode.brcs, Opcode.breq, Opcode.brmi, Opcode.brvs,
                Opcode.brlt, Opcode.brhs, Opcode.brts, Opcode.brie,

                Opcode.brcc, Opcode.brne, Opcode.brpl, Opcode.brvc,
                Opcode.brge, Opcode.brhc, Opcode.brtc, Opcode.brid
            };

            public override AvrInstruction Decode(Avr8Disassembler dasm, ushort wInstr)
            {
                int br = (((wInstr >> 7) & 8) | (wInstr & 7)) & 0xF;
                return dasm.Decode(wInstr, branches[br], "o");
            }
        }
    }
}