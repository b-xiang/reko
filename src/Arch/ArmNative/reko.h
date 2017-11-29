#pragma once
/*
* Copyright (C) 1999-2017 John K�ll�n.
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


// Most of these definitions should be generated from the C# side.

enum class BaseType
{
	Void, 
	
	Bool,

	Byte,
	SByte,
	Char8,

	Int16,
	UInt16,
	Ptr16,
	Word16,

	Int32,
	UInt32,
	Ptr32,
	Word32,

	Int64,
	UInt64,
	Ptr64,
	Word64,

	Word128,

	Real32,
	Real64,
};

enum class ConditionCode
{
	None,
	UGT,	// Unsigned >
	ULE,	// Unsigned <=
	ULT,	// Unsigned <
	GT,		// >
	GE,		// >=
	LT,		// <
	LE,		// <=
	UGE,	// Unsigned >=
	NO,		// No overflow
	NS,		// >= 0
	NE,		// != 
	OV,		// Overflow
	SG,		// < 0
	EQ,		// ==	
	PE,     // Parity even
	PO,     // parity odd

	ALWAYS, // Some architectures have this.
	NEVER,
};

enum class RtlClass
{
	Linear = 1,
	Transfer = 2,
	Conditional = 4,
	Delay = 8,
	Annul = 16,
	Invalid = 32,
	ConditionalTransfer = Conditional | Transfer,
};


// The C++ side never really looks at the objects in the Rtl tree being 
// built, so we can represent them as an opaque handle.
enum HExpr { };

class INativeRtlEmitter : IUnknown
{
public:
	virtual void STDAPICALLTYPE Assign(HExpr dst, HExpr src) = 0;
	virtual void STDAPICALLTYPE Branch(HExpr exp, HExpr dst, RtlClass rtlClass) = 0;
	virtual void STDAPICALLTYPE BranchInMiddleOfInstruction(HExpr exp, HExpr dst, RtlClass) = 0;
	virtual void STDAPICALLTYPE Call(HExpr dst, int bytesOnStack) = 0;
	virtual void STDAPICALLTYPE Goto(HExpr dst) = 0;
	virtual void STDAPICALLTYPE Invalid() = 0;
	virtual void STDAPICALLTYPE Nop()=0;
	virtual void STDAPICALLTYPE Return(int, int) = 0;
	virtual void STDAPICALLTYPE FinishCluster(RtlClass rtlClass, uint64_t address, int32_t mcLength) = 0;
	virtual void STDAPICALLTYPE SideEffect(HExpr) = 0;

	virtual HExpr STDAPICALLTYPE And(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Cast(BaseType type, HExpr a) = 0;
	virtual HExpr STDAPICALLTYPE Comp(HExpr a) = 0;
	virtual HExpr STDAPICALLTYPE Cond(HExpr a) = 0;
	virtual HExpr STDAPICALLTYPE Dpb(HExpr dst, HExpr src, int32_t pos) = 0;
	virtual HExpr STDAPICALLTYPE IAdc(HExpr a, HExpr b, HExpr c) = 0;
	virtual HExpr STDAPICALLTYPE IAdd(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE IMul(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE ISub(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE FAdd(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE FSub(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE FMul(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE FDiv(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Mem(BaseType dt, HExpr ea) = 0;
	virtual HExpr STDAPICALLTYPE Mem8(HExpr ea) = 0;
	virtual HExpr STDAPICALLTYPE Mem16(HExpr ea) = 0;
	virtual HExpr STDAPICALLTYPE Mem32(HExpr ea) = 0;
	virtual HExpr STDAPICALLTYPE Mem64(HExpr ea) = 0;
	virtual HExpr STDAPICALLTYPE Not(HExpr a) = 0;
	virtual HExpr STDAPICALLTYPE Or(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Ror(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Rrc(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Sar(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Slice(HExpr a, int32_t pos, int32_t bits) = 0;
	virtual HExpr STDAPICALLTYPE Shl(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Shr(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Test(ConditionCode cc, HExpr exp) = 0;
	virtual HExpr STDAPICALLTYPE SMul(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE UMul(HExpr a, HExpr b) = 0;
	virtual HExpr STDAPICALLTYPE Xor(HExpr a, HExpr b) = 0;

	virtual HExpr STDAPICALLTYPE Byte(uint8_t) = 0;
	virtual HExpr STDAPICALLTYPE Int16(int16_t) = 0;
	virtual HExpr STDAPICALLTYPE Int32(int32_t) = 0;
	virtual HExpr STDAPICALLTYPE Int64(int64_t) = 0;
	virtual HExpr STDAPICALLTYPE Ptr16(uint16_t) = 0;
	virtual HExpr STDAPICALLTYPE Ptr32(uint32_t) = 0;
	virtual HExpr STDAPICALLTYPE Ptr64(uint64_t) = 0;
	virtual HExpr STDAPICALLTYPE UInt16(uint16_t) = 0;
	virtual HExpr STDAPICALLTYPE UInt32(uint32_t) = 0;
	virtual HExpr STDAPICALLTYPE UInt64(uint64_t) = 0;
	virtual HExpr STDAPICALLTYPE Word16(uint16_t) = 0;
	virtual HExpr STDAPICALLTYPE Word32(uint32_t) = 0;
	virtual HExpr STDAPICALLTYPE Word64(uint64_t) = 0;

	virtual void STDAPICALLTYPE AddArg(HExpr a) = 0;
	virtual HExpr STDAPICALLTYPE Fn(HExpr fn) = 0;
};

class INativeTypeFactory : public IUnknown
{
public:
	virtual HExpr STDAPICALLTYPE ArrayOf(HExpr dt, int32_t length) = 0;

	virtual HExpr STDAPICALLTYPE PtrTo(HExpr dt, int32_t byte_size) = 0;

	virtual void STDAPICALLTYPE BeginStruct(HExpr dt, int32_t byte_size) = 0;
	virtual void STDAPICALLTYPE Field(HExpr dt, int32_t offset, const char * name) = 0;
	virtual HExpr STDAPICALLTYPE EndStruct() = 0;

	virtual void STDAPICALLTYPE BeginFunc(HExpr dt, int32_t byte_size) = 0;
	virtual void STDAPICALLTYPE Parameter(HExpr dt, const char * name) = 0;
	virtual HExpr STDAPICALLTYPE EndFunc() = 0;
};


class INativeRewriter : public IUnknown
{
public:
	virtual STDMETHODIMP Next() = 0;
	virtual int32_t STDAPICALLTYPE  GetCount() = 0; //$DEBUG: used to track object leaks.
};

class INativeRewriterHost: public IUnknown
{
public:
	virtual HExpr STDAPICALLTYPE EnsureRegister(int reg_kind, int reg) = 0;
	virtual HExpr STDAPICALLTYPE EnsureSequence(int regHi, int regLo, BaseType size) = 0;
	virtual HExpr STDAPICALLTYPE EnsureFlagGroup(int baseReg, int bitmask, const char * name, BaseType size) = 0;
	virtual HExpr STDAPICALLTYPE CreateTemporary(BaseType size) = 0;

	virtual void STDAPICALLTYPE Error(uint64_t uAddress, const char * error) = 0;
	virtual HExpr STDAPICALLTYPE EnsurePseudoProcedure(const char *name, BaseType retType, int arity) = 0;
};


class INativeInstructionWriter : public IUnknown
{
public:
	virtual void STDAPICALLTYPE AddAnnotation(const char * a) = 0;
	virtual void STDAPICALLTYPE WriteOpcode(const char * opcode) = 0;
	virtual void STDAPICALLTYPE WriteAddress(const char * formattedAddress, uint64_t uAddr) = 0;
	virtual void STDAPICALLTYPE Tab() = 0;
	virtual void STDAPICALLTYPE Write(char c) = 0;
	virtual void STDAPICALLTYPE Write(uint32_t n) = 0;
	virtual void STDAPICALLTYPE Write(const char * s) = 0;
};

//    static const GUID <<name>> = 
//{ 0x10475e6b, 0xd167, 0x4db3, { 0xb2, 0x11, 0x61, 0xf, 0x60, 0x73, 0xa3, 0x13 } };

enum class MachineInstructionWriterOptions
{
	None = 0,
	ExplicitOperandSize = 1,
	ResolvePcRelativeAddress = 2,
};

class INativeDisassembler : public IUnknown
{
	virtual void * STDAPICALLTYPE NextInstruction() = 0;
	virtual void STDAPICALLTYPE Render(void * instr, INativeInstructionWriter * writer, MachineInstructionWriterOptions options) = 0;
	virtual void STDAPICALLTYPE DestroyInstruction(void * instr) = 0;
};

struct NativeRegister
{
	char * Name;
	int32_t Number;
	int32_t BitSize;
};

class INativeArchitecture : public IUnknown
{
public:
	virtual void STDAPICALLTYPE GetAllRegisters(int * pcRegs, const NativeRegister ** ppRegs) = 0;
	virtual INativeDisassembler * STDAPICALLTYPE CreateDisassembler(const uint8_t * bytes, int length, int offset, uint64_t uAddr) = 0;
};