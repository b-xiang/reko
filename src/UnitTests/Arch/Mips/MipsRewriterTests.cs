﻿#region License
/* 
 * Copyright (C) 1999-2015 John Källén.
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

using Reko.Arch.Mips;
using Reko.Core;
using Reko.Core.Rtl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reko.Core.Types;

namespace Reko.UnitTests.Arch.Mips
{
    [TestFixture]
    public class MipsRewriterTests : RewriterTestBase
    {
        static MipsProcessorArchitecture arch = new MipsBe32Architecture();
        private MipsDisassembler dasm;

        public override IProcessorArchitecture Architecture { get { return arch; } }

        public override Address LoadAddress { get { return Address.Ptr32(0x00100000); } }

        private void RunTest(params string[] bitStrings)
        {
            var bytes = bitStrings.Select(bits => base.ParseBitPattern(bits))
                .SelectMany(u => new byte[] { (byte)(u >> 24), (byte)(u >> 16), (byte)(u >> 8), (byte)u })
                .ToArray();
            dasm = new MipsDisassembler(arch, new BeImageReader(new LoadedImage(Address.Ptr32(0x00100000), bytes), 0));
        }

        private void AssertCode(uint instr, params string[] sExp)
        {
            Rewrite(instr);
            AssertCode(sExp);
        }

        protected override LoadedImage RewriteCode(uint[] words)
        {
            byte[] bytes = words.SelectMany(w => new byte[]
            {
                (byte) (w >> 24),
                (byte) (w >> 16),
                (byte) (w >> 8),
                (byte) w
            }).ToArray();
            var image = new LoadedImage(LoadAddress, bytes);
            dasm = new MipsDisassembler(arch, image.CreateBeReader(LoadAddress));
            return image;
        }

        protected override IEnumerable<RtlInstructionCluster> GetInstructionStream(Frame frame, IRewriterHost host)
        {
            return new MipsRewriter(arch, dasm, frame, host);
        }

        [Test]
        public void MipsRw_lh()
        {
            RunTest("100001 01001 00011 1111111111001000");
            AssertCode(
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r3 = (word32) Mem0[r9 - 0x00000038:int16]");
        }

        [Test]
        public void MipsRw_lhu()
        {
            RunTest("100101 01011 01101 1111111111111000");
            AssertCode(
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r13 = (word32) Mem0[r11 - 0x00000008:word16]");
        }

        [Test]
        public void MipsRw_lui()
        {
            RunTest("001111 00000 00011 1111111111001000");
            AssertCode(
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r3 = 0xFFC80000");
        }

        [Test]
        public void MipsRw_ori_r0()
        {
            RunTest("001101 00000 00101 1111100000100111");
            AssertCode(
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r5 = 0x0000F827");
        }

        [Test]
        public void MipsRw_addi_r0()
        {
            RunTest("001000 00000 00010 1111111111111000");
            AssertCode(
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r2 = -8");
        }

        [Test]
        public void MipsRw_add()
        {
            RunTest("000000 00001 00010 00011 00000 100000");
            AssertCode(
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r3 = r1 + r2");
        }

        [Test]
        public void MipsRw_andi_0()
        {
            RunTest("001100 00000 00101 0000000000000000");
            AssertCode(
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r5 = 0x00000000");
        }

        [Test]
        public void MipsRw_bgtz()
        {
            RunTest("000111 00011 00000 1111111111111110");
            AssertCode(
                "0|T--|00100000(4): 1 instructions",
                "1|TD-|if (r3 > 0x00000000) branch 000FFFFC");
        }

        [Test]
        public void MipsRw_j()
        {
            RunTest("000010 11111111111111111111111111");
            AssertCode(
                "0|T--|00100000(4): 1 instructions",
                "1|TD-|goto 0FFFFFFC");
        }

        [Test]
        public void MipsRw_sw()
        {
            AssertCode(0xAFBF0020,
                "0|L--|00100000(4): 1 instructions",
                "1|L--|Mem0[sp + 0x00000020:word32] = ra");
        }

        [Test]
        public void MipsRw_jal()
        {
            AssertCode(0x0C009B2C,
                "0|T--|00100000(4): 1 instructions",
                "1|TD-|call 00026CB0 (0)");
        }

        [Test]
        public void MipsRw_srl()
        {
            AssertCode(0x00024c02,
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r9 = r2 >>u 0x10");
        }

        [Test]
        public void MipsRw_nop()
        {
            AssertCode(0x00000000,
                "0|L--|00100000(4): 0 instructions");
        }

        [Test]
        public void MipsRw_lw()
        {
            AssertCode(0x8fb30010,   // lw s3,16(sp)
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r19 = Mem0[sp + 0x00000010:word32]");
            AssertCode(0x8fb3FFF0,   // lw s3,16(sp)
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r19 = Mem0[sp - 0x00000010:word32]");
        }

        [Test]
        public void MipsRw_beq()
        {
            AssertCode(0x10300005,
                "0|T--|00100000(4): 1 instructions",
                "1|TD-|if (r1 == r16) branch 00100018");
        }

        [Test]
        public void MipsRw_Nor()
        {
            AssertCode(0x01004027, // nor t0,t0,zero	
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r8 = ~r8");
        }

        [Test]
        public void MipsRw_jr()
        {
            AssertCode(0x01000008,// jr t0	      
                "0|T--|00100000(4): 1 instructions",
                "1|TD-|goto r8");
        }

        [Test]
        public void MipsRw_xor()
        {
            AssertCode(0x01285026, // xor t2,t1,t0
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r10 = r9 ^ r8");
        }

        [Test]
        public void MipsRw_jalr()
        {
            AssertCode(0x0120f809,  // jalr t1
                "0|T--|00100000(4): 1 instructions",
                "1|TD-|call r9 (0)");
        }

        [Test]
        public void MipsRw_sltu()
        {
            AssertCode(0x0211402B,  // sltu t0,s0,s1
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r8 = (word32) (r16 <u r17)");
        }

        [Test]
        public void MipsRw_jr_returns()
        {
            AssertCode(0x03E00008, // jr ra
                "0|T--|00100000(4): 1 instructions",
                "1|TD-|return (0,0)");
        }

        [Test]
        public void MipsRw_sb()
        {
            AssertCode(0xA128D958, // sb t0,-9896(t1)
                "0|L--|00100000(4): 1 instructions",
                "1|L--|Mem0[r9 - 0x000026A8:byte] = (byte) r8");
        }

        [Test]
        [Ignore]
        public void MipsRw_tge()
        {
            AssertCode(0x00F000F0,  // tge a3,s0,0x3
                "0|L--|00100000(4): 1 instructions",
                "1|L--|if (a3 >= s0) __trap(0x03)");
        }

        [Test]
        public void MipsRw_br()
        {
            AssertCode(0x1000ffc2,  // b loc_00026e0
                "0|T--|00100000(4): 1 instructions",
                "1|TD-|goto 000FFF0C");
        }

        [Test]
        public void MipsRw_slti()
        {
            AssertCode(0x29690070,
                "0|L--|00100000(4): 1 instructions",
                "1|L--|r9 = (word32) (r11 < 112)");
        }
    }
}
