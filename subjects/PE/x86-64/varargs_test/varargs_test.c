// varargs_test.c
// Generated by decompiling varargs_test.exe
// using Reko decompiler version 0.8.1.0.

#include "varargs_test.h"

// 0000000140001000: Register word64 fn0000000140001000(Register word64 rbx, Register word64 rsi, Register word64 rdi, Register out ptr64 rcxOut, Register out ptr64 rdxOut, Register out ptr64 r8Out, Register out ptr64 r9Out, Register out Eq_n xmm0Out)
word64 fn0000000140001000(word64 rbx, word64 rsi, word64 rdi, ptr64 & rcxOut, ptr64 & rdxOut, ptr64 & r8Out, ptr64 & r9Out, union Eq_n & xmm0Out)
{
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	ui64 rax_n = globals->t40003000 ^ fp - 200;
	Eq_n xmm0_n = globals->t40002260;
	Eq_n stackArg8 = <invalid>;
	Eq_n stackArg16 = <invalid>;
	Eq_n stackArg24 = <invalid>;
	Eq_n stackArg32 = <invalid>;
	fn0000000140001140(rax_n, 0x140002210, 0x03, rbx, rsi, rdi, 0x140002250, 0x140002240, stackArg8, stackArg16, stackArg24, stackArg32);
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	Eq_n stackArg8 = <invalid>;
	Eq_n stackArg16 = <invalid>;
	Eq_n stackArg24 = <invalid>;
	Eq_n stackArg32 = <invalid>;
	ptr64 r9_n = fn00000001400010D0(fp - 0x80, 0x140002228, fp - 0x7C, rbx, rsi, rdi, fp - 0x70, fp - 0x50, stackArg8, stackArg16, stackArg24, stackArg32);
	ptr64 r8_n;
	ptr64 rcx_n;
	ptr64 rdx_n;
	word64 rax_n = fn00000001400011B0(rax_n ^ fp - 200, rbx, rsi, rdi, qwLocD0, out rcx_n, out rdx_n, out r8_n);
	rcxOut = rcx_n;
	rdxOut = rdx_n;
	r8Out = r8_n;
	r9Out = r9_n;
	xmm0Out = xmm0_n;
	return rax_n;
}

// 00000001400010C0: Register ptr64 fn00000001400010C0()
ptr64 fn00000001400010C0()
{
	return 0x140003628;
}

// 00000001400010D0: Register ptr64 fn00000001400010D0(Register ptr64 rax, Register ptr64 rcx, Register ptr64 rdx, Register word64 rbx, Register word64 rsi, Register word64 rdi, Register ptr64 r8, Register ptr64 r9, Stack Eq_n qwArg08, Stack Eq_n qwArg10, Stack Eq_n qwArg18, Stack Eq_n qwArg20)
ptr64 fn00000001400010D0(ptr64 rax, ptr64 rcx, ptr64 rdx, word64 rbx, word64 rsi, word64 rdi, ptr64 r8, ptr64 r9, Eq_n qwArg08, Eq_n qwArg10, Eq_n qwArg18, Eq_n qwArg20)
{
	word64 rax_n;
	_acrt_iob_func();
	ui64 * rax_n = fn00000001400010C0();
	ptr64 r9_n;
	_stdio_common_vfscanf();
	return r9_n;
}

// 0000000140001130: Register ptr64 fn0000000140001130()
ptr64 fn0000000140001130()
{
	return 0x140003620;
}

// 0000000140001140: void fn0000000140001140(Register ui64 rax, Register ptr64 rcx, Register uint64 rdx, Register word64 rbx, Register word64 rsi, Register word64 rdi, Register ptr64 r8, Register ptr64 r9, Stack Eq_n qwArg08, Stack Eq_n qwArg10, Stack Eq_n qwArg18, Stack Eq_n qwArg20)
void fn0000000140001140(ui64 rax, ptr64 rcx, uint64 rdx, word64 rbx, word64 rsi, word64 rdi, ptr64 r8, ptr64 r9, Eq_n qwArg08, Eq_n qwArg10, Eq_n qwArg18, Eq_n qwArg20)
{
	word64 rax_n;
	_acrt_iob_func();
	ui64 * rax_n = fn0000000140001130();
	_stdio_common_vfprintf();
}

// 00000001400011B0: Register word64 fn00000001400011B0(Register Eq_n rcx, Register word64 rbx, Register word64 rsi, Register word64 rdi, Stack word64 qwArg00, Register out Eq_n rcxOut, Register out ptr64 rdxOut, Register out ptr64 r8Out)
word64 fn00000001400011B0(Eq_n rcx, word64 rbx, word64 rsi, word64 rdi, word64 qwArg00, union Eq_n & rcxOut, ptr64 & rdxOut, ptr64 & r8Out)
{
	if (rcx == globals->t40003000)
	{
		Eq_n rcx_n = __rol(rcx, 0x10);
		if (((word16) rcx_n & ~0x00) == 0x00)
		{
			rcxOut = rcx_n;
			rdxOut = rdx;
			r8Out = r8;
			return rax;
		}
		rcx = __ror(rcx_n, 0x10);
	}
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	Eq_n stackArg8 = <invalid>;
	ptr64 r8_n;
	Eq_n rcx_n;
	ptr64 rdx_n;
	word64 rax_n = fn000000014000147C(rcx, rbx, rsi, rdi, qwArg00, stackArg8, out rcx_n, out rdx_n, out r8_n);
	rcxOut = rcx_n;
	rdxOut = rdx_n;
	r8Out = r8_n;
	return rax_n;
}

// 00000001400011D4: void fn00000001400011D4(Register word64 rax, Register word64 rdx, Register word64 rbx, Register ptr64 rbp, Register word64 rsi, Register word64 rdi, Register ui64 r8, Register ptr64 r9, Register Eq_n xmm0, Register Eq_n xmm1)
void fn00000001400011D4(word64 rax, word64 rdx, word64 rbx, ptr64 rbp, word64 rsi, word64 rdi, ui64 r8, ptr64 r9, Eq_n xmm0, Eq_n xmm1)
{
	set_app_type();
	fn0000000140001920();
	_set_fmode(dwLoc28);
	int32 * rax_n = __p__commode();
	*rax_n = (word32) fn0000000140001ABC();
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	Eq_n stackArg8 = <invalid>;
	word64 r8_n;
	word64 r9_n;
	ptr64 rbp_n;
	int32 * rbx_n;
	word64 rdx_n;
	word128 xmm0_n;
	word128 xmm1_n;
	Eq_n rax_n = fn000000014000164C(0x01, rdx, rax_n, rbp, r8, r9, xmm0, xmm1, stackArg8, out rdx_n, out rbx_n, out rbp_n, out r8_n, out r9_n, out xmm0_n, out xmm1_n);
	if ((byte) rax_n != 0x00)
	{
		// Failed to bind call argument.
		// Please report this issue at https://github.com/uxmal/reko
		// Failed to bind call argument.
		// Please report this issue at https://github.com/uxmal/reko
		Eq_n stackArg8 = <invalid>;
		Eq_n stackArg16 = <invalid>;
		fn0000000140001B5C(rbx_n, rsi, rdi, stackArg8, stackArg16);
		word64 rdx_n = fn0000000140001854(rax_n, 0x140001BA8, rbx_n);
		word64 rax_n = fn0000000140001918();
		word64 rax_n;
		word64 r8_n;
		word64 rdx_n;
		word64 r9_n;
		configure_narrow_argv();
		if ((word32) rax_n == 0x00)
		{
			fn0000000140001928(qwLoc30);
			if ((word32) (word64) fn0000000140001958() != 0x00)
				__setusermatherr(&globals->t40001ABC);
			fn0000000140001DD0();
			fn0000000140001DD0();
			word64 rax_n = fn0000000140001ABC();
			word64 rcx_n;
			word64 rax_n;
			word64 r8_n;
			word64 rdx_n;
			word64 r9_n;
			configthreadlocale();
			byte al_n = fn0000000140001938();
			word64 rax_n = DPB(rax_n, al_n, 0);
			if (al_n != 0x00)
				initialize_narrow_environment();
			fn0000000140001ABC();
			return;
		}
	}
	else
	{
		// Failed to bind call argument.
		// Please report this issue at https://github.com/uxmal/reko
		Eq_n stackArg8 = <invalid>;
		fn0000000140001974(0x07, rbx_n, rbp_n, qwLoc30, stackArg8, qwLoc20, out rcx_n, out rdx_n, out r8_n);
		int3();
	}
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	Eq_n stackArg8 = <invalid>;
	fn0000000140001974(0x07, rbx_n, rbp_n, qwLoc30, stackArg8, qwLoc20, out rcx_n, out rdx_n, out r8_n);
	int3();
	int3();
	fn0000000140001290();
}

// 0000000140001290: void fn0000000140001290()
void fn0000000140001290()
{
	fn000000014000193C();
}

// 00000001400012A0: void fn00000001400012A0()
void fn00000001400012A0()
{
	fn0000000140001B14(tLoc30);
	word64 rax_n = fn0000000140001ABC();
	set_new_mode();
}

// 00000001400012BC: void fn00000001400012BC(Register word64 rax, Register ptr64 rdx, Register (ptr64 int32) rbx, Register ptr64 rbp, Register word64 rsi, Register word64 rdi, Register word64 r8, Register word64 r9, Register word128 xmm0, Stack Eq_n qwArg08, Stack Eq_n qwArg10)
void fn00000001400012BC(word64 rax, ptr64 rdx, int32 * rbx, ptr64 rbp, word64 rsi, word64 rdi, word64 r8, word64 r9, word128 xmm0, Eq_n qwArg08, Eq_n qwArg10)
{
	struct Eq_n * rax_n = DPB(rax, al, 0);
	fn0000000140001600(ecx, edx);
	if (al == 0x00)
	{
		// Failed to bind call argument.
		// Please report this issue at https://github.com/uxmal/reko
		Eq_n stackArg8 = <invalid>;
		rax_n = fn0000000140001974(0x07, rbx, rbp, qwLoc40, stackArg8, qwLoc30, out rcx_n, out rdx, out r8);
		int3();
		al = (byte) rax_n;
	}
	fn00000001400015C4(gs);
	uint64 rcx_n = (uint64) globals->dw400035B0;
	struct Eq_n * rax_n = DPB(rax_n, al, 0);
	word64 rsi_n = DPB(rsi, 0x00, 0);
	byte bLoc18_n = 0x00;
	int32 * rbx_n = DPB(rbx, al, 0);
	word32 ecx_n = (word32) rcx_n;
	if (ecx_n == 0x01)
	{
		// Failed to bind call argument.
		// Please report this issue at https://github.com/uxmal/reko
		Eq_n stackArg8 = <invalid>;
		rax_n = fn0000000140001974(0x07, rbx_n, rbp, qwLoc40, stackArg8, qwLoc30, out rcx_n, out rdx, out r8);
		ecx_n = (word32) rcx_n;
	}
	Eq_n rcx_n = DPB(rcx_n, ecx_n, 0);
	if (ecx_n == 0x00)
	{
		globals->dw400035B0 = 0x01;
		int32 eax_n = _initterm_e(&globals->t400021B8, &globals->t400021D0);
		rax_n = DPB(rax_n, eax_n, 0);
		if (eax_n != 0x00)
			return;
		_initterm(&globals->t400021A0, &globals->t400021B0);
		globals->dw400035B0 = 0x02;
		rdx = 0x1400021B0;
		rcx_n.u1 = 0x1400021A0;
	}
	else
	{
		rsi_n = DPB(rsi, 0x01, 0);
		bLoc18_n = 0x01;
	}
	byte sil_n = (byte) rsi_n;
	fn00000001400017B4(cl);
	fn0000000140001964();
	word64 rsi_n = DPB(rsi_n, sil_n, 0);
	Eq_n rcx_n = DPB(rcx_n, al, 0);
	if (rax_n->t0000 != 0x00)
	{
		rax_n = fn0000000140001718(rax_n, out rcx_n, out rdx, out r8, out r9);
		if ((byte) rax_n != 0x00)
		{
			Eq_n rbx_n = rax_n->t0000;
			fn0000000140001BF4();
			rbx_n();
		}
	}
	struct Eq_n * rax_n;
	fn000000014000196C();
	rax_n = rax_n;
	if (rax_n->t0000 != 0x00)
	{
		rax_n = fn0000000140001718(rax_n, out rcx_n, out rdx, out r8, out r9);
		if ((byte) rax_n != 0x00)
			register_thread_local_exe_atexit_callback();
	}
	word64 rcx_n;
	word64 rax_n;
	word64 rdx_n;
	word64 r8_n;
	word64 r9_n;
	_p___argv();
	word64 rcx_n;
	word64 rax_n;
	word64 rdx_n;
	word64 r8_n;
	word64 r9_n;
	_p___argc();
	get_initial_narrow_environment();
	word64 r8_n;
	word64 r9_n;
	word64 rcx_n;
	word64 rdx_n;
	word128 xmm0_n;
	word64 rax_n = fn0000000140001000(rax_n, rsi_n, rax_n, out rcx_n, out rdx_n, out r8_n, out r9_n, out xmm0_n);
	fn0000000140001AC0(rax);
	uint64 rbx_n = (uint64) (word32) rax_n;
	word32 ebx_n = (word32) rbx_n;
	if ((byte) rax_n != 0x00)
	{
		if (sil_n == 0x00)
			cexit();
		fn00000001400017D8(dl);
	}
	else
		exit((uint64) ebx_n);
}

// 0000000140001434: Register Eq_n Win32CrtStartup()
Eq_n Win32CrtStartup()
{
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	Eq_n stackArg8 = <invalid>;
	Eq_n stackArg32 = <invalid>;
	word64 rax_n = fn000000014000186C(stackArg8, qwLoc20, dwLoc18, dwLoc14, stackArg32);
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	Eq_n stackArg8 = <invalid>;
	Eq_n stackArg16 = <invalid>;
	fn00000001400012BC(rax_n, rdx, rbx, rbp, rsi, rdi, r8, r9, xmm0, stackArg8, stackArg16);
	return <invalid>;
}

// 0000000140001448: Register Eq_n fn0000000140001448(Stack Eq_n tArg00, Stack Eq_n tArg08)
Eq_n fn0000000140001448(Eq_n tArg00, Eq_n tArg08)
{
	SetUnhandledExceptionFilter(tLoc28);
	UnhandledExceptionFilter(tLoc28);
	GetCurrentProcess();
	TerminateProcess(tArg00, tArg08);
	return <invalid>;
}

// 000000014000147C: Register word64 fn000000014000147C(Register Eq_n rcx, Register word64 rbx, Register word64 rsi, Register word64 rdi, Stack word64 qwArg00, Stack Eq_n qwArg08, Register out ptr64 rcxOut, Register out ptr64 rdxOut, Register out ptr64 r8Out)
word64 fn000000014000147C(Eq_n rcx, word64 rbx, word64 rsi, word64 rdi, word64 qwArg00, Eq_n qwArg08, ptr64 & rcxOut, ptr64 & rdxOut, ptr64 & r8Out)
{
	word64 qwLoc38_n = SEQ(dwLoc34, tLoc38);
	Eq_n tLoc38_n = SLICE(qwLoc38_n, DWORD, 0);
	if (IsProcessorFeaturePresent(tLoc38) == 0x00)
	{
		fn0000000140001550(&globals->t400030E0, rbx, rsi, rdi, qwLoc38_n);
		globals->qw400031D8 = qwArg00;
		globals->ptr40003178 = fp + 0x08;
		globals->qw40003050 = globals->qw400031D8;
		globals->t40003160 = rcx;
		globals->dw40003040 = 0xC0000409;
		globals->dw40003044 = 0x01;
		globals->dw40003058 = 0x01;
		globals->qw40003060 = 0x02;
		word64 rax_n = DPB(0x08, fn0000000140001448(tLoc40, tLoc38_n), 0);
		rcxOut = rcx_n;
		rdxOut = rdx_n;
		r8Out = r8_n;
		return rax_n;
	}
	else
		__fastfail(0x02);
}

// 0000000140001550: void fn0000000140001550(Register (ptr64 Eq_n) rcx, Register word64 rbx, Register word64 rsi, Register word64 rdi, Stack word64 qwArg08)
void fn0000000140001550(struct Eq_n * rcx, word64 rbx, word64 rsi, word64 rdi, word64 qwArg08)
{
	RtlCaptureContext(tLoc58);
	Eq_n rsi_n = rcx->t00F8;
	uint64 rdi_n = 0x00;
	do
	{
		word32 edi_n = (word32) rdi_n;
		Eq_n rax_n = RtlLookupFunctionEntry(rsi_n, fp + 0x08, null);
		if (rax_n == null)
			return;
		ui64 v16_n = qwLoc20 & 0x00;
		KERNEL32.dll!RtlVirtualUnwind();
		rdi_n = (uint64) (edi_n + 0x01);
		qwLoc20 = v16_n;
	} while ((word32) rdi_n < 0x02);
}

// 00000001400015C4: void fn00000001400015C4(Register (ptr32 Eq_n) gs)
void fn00000001400015C4(struct Eq_n * gs)
{
	if ((word32) fn0000000140001DC4() != 0x00)
	{
		word64 rcx_n = gs->ptr0030->qw0008;
		do
		{
			__lock();
			word64 rax_n;
			if (!__cmpxchg(globals->qw400035B8, rcx_n, 0x00, out rax_n))
				goto l00000001400015F2;
		} while (rcx_n != rax_n);
	}
	else
	{
l00000001400015F2:
	}
}

// 0000000140001600: void fn0000000140001600(Register word32 ecx, Register word32 edx)
void fn0000000140001600(word32 ecx, word32 edx)
{
	byte al_n = (byte) (word32) globals->b400035F0;
	if (ecx == 0x00)
		al_n = 0x01;
	globals->b400035F0 = al_n;
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	Eq_n stackArg8 = <invalid>;
	Eq_n stackArg16 = <invalid>;
	Eq_n stackArg24 = <invalid>;
	fn0000000140001BFC(edx, 0x01, stackArg8, stackArg16, stackArg24);
	if (fn0000000140001938() == 0x00)
		return;
	if (fn0000000140001938() != 0x00)
		return;
	fn0000000140001938();
}

// 000000014000164C: Register (ptr64 Eq_n) fn000000014000164C(Register up32 ecx, Register word64 rdx, Register (ptr64 int32) rbx, Register ptr64 rbp, Register ui64 r8, Register ptr64 r9, Register Eq_n xmm0, Register Eq_n xmm1, Stack Eq_n qwArg08, Register out Eq_n rdxOut, Register out (ptr64 int32) rbxOut, Register out ptr64 rbpOut, Register out Eq_n r8Out, Register out ptr64 r9Out, Register out Eq_n xmm0Out, Register out Eq_n xmm1Out)
struct Eq_n * fn000000014000164C(up32 ecx, word64 rdx, int32 * rbx, ptr64 rbp, ui64 r8, ptr64 r9, Eq_n xmm0, Eq_n xmm1, Eq_n qwArg08, union Eq_n & rdxOut, int32 & rbxOut, ptr64 & rbpOut, union Eq_n & r8Out, ptr64 & r9Out, union Eq_n & xmm0Out, union Eq_n & xmm1Out)
{
	int32 * rbx_n = (uint64) ecx;
	word32 ebx_n = (word32) rbx_n;
	uint64 rbx_n = DPB(rbx_n, ebx_n, 0);
	if (ecx > 0x01)
	{
		// Failed to bind call argument.
		// Please report this issue at https://github.com/uxmal/reko
		Eq_n stackArg8 = <invalid>;
		struct Eq_n * rcx_n;
		fn0000000140001974(0x05, rbx_n, fp - 0x08, qwLoc50, stackArg8, qwLoc40, out rcx_n, out rdx_n, out r8_n);
		int3();
		int3();
		int3();
		int3();
		Eq_n r8_n;
		Eq_n rdx_n;
		ptr64 r9_n;
		struct Eq_n * rax_n = fn0000000140001718(rcx_n, out rcx_n, out rdx_n, out r8_n, out r9_n);
		rdxOut = rdx_n;
		rbxOut = rbx_n;
		rbpOut = fp - 0x08;
		r8Out = r8_n;
		r9Out = r9_n;
		xmm0Out = xmm0;
		xmm1Out = xmm1;
		return rax_n;
	}
	else
	{
		Eq_n rdx_n;
		struct Eq_n * rax_n;
		Eq_n r8_n;
		word64 rax_n = fn0000000140001DC4();
		if ((word32) rax_n != 0x00 && ebx_n == 0x00)
		{
			word64 rax_n;
			initialize_onexit_table();
			if ((word32) rax_n != 0x00)
				rax_n = DPB(rax_n, 0x00, 0);
			else
			{
				word64 rax_n;
				initialize_onexit_table();
				rax_n = DPB(rax_n, (word32) rax_n == 0x00, 0);
			}
		}
		else
		{
			rdx_n = globals->t40003000;
			uint64 rax_n = (uint64) ((word32) (uint64) (word32) rdx_n & 0x3F);
			r8_n = __ror(r8 | ~0x00, (byte) (uint64) (0x40 - (word32) rax_n)) ^ rdx_n;
			globals->t400035C0 = SEQ(r8_n, r8_n);
			globals->t400035D0 = r8_n;
			Eq_n owLoc28_n = SEQ(r8_n, r8_n);
			globals->t400035D8 = owLoc28_n;
			globals->t400035E8 = r8_n;
			rax_n = DPB(rax_n, 0x01, 0);
			xmm0 = owLoc28_n;
			xmm1 = DPB(xmm1, r8_n, 0);
		}
		rdxOut = rdx_n;
		rbxOut = rbx;
		rbpOut = rbp;
		r8Out = r8_n;
		r9Out = r9;
		xmm0Out = xmm0;
		xmm1Out = xmm1;
		return rax_n;
	}
}

// 0000000140001718: Register uint64 fn0000000140001718(Register (ptr64 Eq_n) rcx, Register out (ptr64 Eq_n) rcxOut, Register out (ptr64 Eq_n) rdxOut, Register out (ptr64 Eq_n) r8Out, Register out (ptr64 Eq_n) r9Out)
uint64 fn0000000140001718(struct Eq_n * rcx, struct Eq_n & rcxOut, struct Eq_n & rdxOut, struct Eq_n & r8Out, struct Eq_n & r9Out)
{
	uint64 rax_n;
	struct Eq_n * r8_n = rcx;
	union Eq_n * rax_n = (union Eq_n *) 23117;
	if (globals->w40000000 == 23117)
	{
		rax_n = (int64) globals->dw4000003C;
		rdx = &globals->w40000000;
		rcx = rax_n + 0x0000000140000000;
		if (rcx->t0000 == 0x4550)
		{
			rax_n = (union Eq_n *) 0x020B;
			if (rcx->w0018 == 0x020B)
			{
				struct Eq_n * rax_n = DPB(0x020B, (word32) rcx->w0006, 0);
				rdx = rcx + 0x18 + DPB(0x020B, (word32) Mem0[rcx + 0x14:word16], 0);
				r8_n = rcx - 0x140000000;
				rcx = rax_n * 0x05;
				r9 = rdx + rax_n * 0x28;
				for (; rdx != r9; rdx += 0x28)
				{
					rcx = (uint64) rdx->dw000C;
					word32 ecx_n = (word32) rcx;
					rcx = DPB(rcx, ecx_n, 0);
					if (r8_n >= rcx)
					{
						rax_n = (uint64) ((word32) (uint64) rdx->dw0008 + ecx_n);
						if (r8_n < rax_n)
							goto l000000014000178F;
					}
				}
				rdx = null;
l000000014000178F:
				if (rdx == null)
					rax_n = DPB(rax_n, 0x00, 0);
				else if (rdx->dw0024 < 0x00)
					rax_n = DPB(rax_n, 0x00, 0);
				else
					rax_n = DPB(rax_n, 0x01, 0);
l00000001400017AC:
				rcxOut = rcx;
				rdxOut = rdx;
				r8Out = r8_n;
				r9Out = r9;
				return rax_n;
			}
		}
	}
	rax_n = DPB(rax_n, 0x00, 0);
	goto l00000001400017AC;
}

// 00000001400017B4: void fn00000001400017B4(Register byte cl)
void fn00000001400017B4(byte cl)
{
	if ((word32) fn0000000140001DC4() != 0x00 && cl == 0x00)
		globals->qw400035B8 = 0x00;
}

// 00000001400017D8: void fn00000001400017D8(Register byte dl)
void fn00000001400017D8(byte dl)
{
	if (globals->b400035F0 == 0x00 || dl == 0x00)
	{
		fn0000000140001938();
		fn0000000140001938();
	}
}

// 0000000140001804: Register Eq_n fn0000000140001804(Register Eq_n rax, Register Eq_n rcx, Register (ptr64 int32) rbx, Register out ptr64 rdxOut)
Eq_n fn0000000140001804(Eq_n rax, Eq_n rcx, int32 * rbx, ptr64 & rdxOut)
{
	ptr64 rdx_n;
	word32 eax_n;
	Eq_n rdx_n = globals->t40003000;
	Eq_n rdx_n = __ror(rdx_n ^ globals->t400035C0, (byte) (uint64) ((word32) (uint64) (word32) rdx_n & 0x3F));
	if (rdx_n == ~0x00)
	{
		word64 rax_n;
		crt_atexit();
		eax_n = (word32) rax_n;
	}
	else
	{
		word64 rax_n;
		register_onexit_function();
		eax_n = (word32) rax_n;
	}
	Eq_n rcx_n = 0x00;
	if (eax_n == 0x00)
		rcx_n = rcx;
	rdxOut = rdx_n;
	return rcx_n;
}

// 0000000140001854: Register word64 fn0000000140001854(Register Eq_n rax, Register Eq_n rcx, Register (ptr64 int32) rbx)
word64 fn0000000140001854(Eq_n rax, Eq_n rcx, int32 * rbx)
{
	word64 rdx_n;
	fn0000000140001804(rax, rcx, rbx, out rdx_n);
	return rdx_n;
}

// 000000014000186C: Register word64 fn000000014000186C(Stack Eq_n qwArg08, Stack ui64 qwArg10, Stack Eq_n qwArg18, Stack word32 dwArg1C, Stack Eq_n qwArg20)
word64 fn000000014000186C(Eq_n qwArg08, ui64 qwArg10, Eq_n qwArg18, word32 dwArg1C, Eq_n qwArg20)
{
	Eq_n rax_n = globals->t40003000;
	if (rax_n == 769630770)
	{
		GetSystemTimeAsFileTime(tLoc28);
		ui64 v14_n = qwArg10 & 0x00 ^ (uint64) GetCurrentThreadId() ^ (uint64) GetCurrentProcessId();
		QueryPerformanceCounter(tLoc28);
		ui64 rax_n = (uint64) dwArg18 << 0x20 ^ SEQ(dwArg1C, dwArg18) ^ v14_n ^ fp + 0x08;
		rax_n = rax_n & 0xFFFFFFFF;
		if ((rax_n & 0xFFFFFFFF) == 769630770)
			rax_n.u0 = 769630771;
		globals->t40003000 = rax_n;
	}
	word64 rax_n = ~rax_n;
	globals->qw40003008 = rax_n;
	return rax_n;
}

// 0000000140001918: Register uint64 fn0000000140001918()
uint64 fn0000000140001918()
{
	return 0x01;
}

// 0000000140001920: void fn0000000140001920()
void fn0000000140001920()
{
}

// 0000000140001928: void fn0000000140001928(Stack Eq_n tArg00)
void fn0000000140001928(Eq_n tArg00)
{
	InitializeSListHead(tArg00);
}

// 0000000140001938: Register byte fn0000000140001938()
byte fn0000000140001938()
{
	return 0x01;
}

// 000000014000193C: void fn000000014000193C()
void fn000000014000193C()
{
	ui64 * rax_n = fn0000000140001130();
	*rax_n |= 0x04;
	ui64 * rax_n = fn00000001400010C0();
	*rax_n |= 0x02;
}

// 0000000140001958: Register word32 fn0000000140001958()
word32 fn0000000140001958()
{
	return (word32) (uint64) (globals->dw40003014 == 0x00);
}

// 0000000140001964: void fn0000000140001964()
void fn0000000140001964()
{
}

// 000000014000196C: void fn000000014000196C()
void fn000000014000196C()
{
}

// 0000000140001974: Register Eq_n fn0000000140001974(Register word32 ecx, Register (ptr64 int32) rbx, Register ptr64 rbp, Stack Eq_n qwArg00, Stack Eq_n qwArg08, Stack word64 qwArg10, Register out Eq_n rcxOut, Register out Eq_n rdxOut, Register out Eq_n r8Out)
Eq_n fn0000000140001974(word32 ecx, int32 * rbx, ptr64 rbp, Eq_n qwArg00, Eq_n qwArg08, word64 qwArg10, union Eq_n & rcxOut, union Eq_n & rdxOut, union Eq_n & r8Out)
{
	word32 ebx_n = (word32) (uint64) ecx;
	Eq_n ptrLoc05C8_n = SEQ(dwLoc05C4, tLoc05C8);
	Eq_n tLoc05C8_n = SLICE(ptrLoc05C8_n, DWORD, 0);
	word32 dwLoc05C4_n = SLICE(ptrLoc05C8_n, word32, 32);
	if (IsProcessorFeaturePresent(tLoc05C8) == 0x00)
	{
		globals->dw40003610 &= 0x00;
		memset(ptrLoc05C8_n, dwLoc05C0, tLoc05B8);
		RtlCaptureContext(ptrLoc05C8_n);
		Eq_n rax_n = RtlLookupFunctionEntry(qwLoc03E0, fp + 0x10, null);
		if (rax_n != null)
		{
			KERNEL32.dll!RtlVirtualUnwind();
			ptrLoc05C8_n = SEQ(dwLoc05C4_n, tLoc05C8_n);
		}
		memset(ptrLoc05C8_n, dwLoc05C0, tLoc05B8);
		Eq_n eax_n = IsDebuggerPresent();
		SetUnhandledExceptionFilter(ptrLoc05C8_n);
		Eq_n eax_n = UnhandledExceptionFilter(ptrLoc05C8_n);
		Eq_n rax_n = DPB(fp - 0x04D8, eax_n, 0);
		if (eax_n == 0x00)
		{
			rax_n.u0 = (uint64) (0x00 - (-(eax_n == 0x01) == 0x00));
			globals->dw40003610 &= (word32) rax_n;
		}
		rcxOut = fp - 0x0588;
		rdxOut.u0 = 0x00;
		r8Out.u0 = 0x98;
		return rax_n;
	}
	else
		__fastfail((word32) (uint64) ebx_n);
}

// 0000000140001ABC: Register uint64 fn0000000140001ABC()
uint64 fn0000000140001ABC()
{
	return 0x00;
}

// 0000000140001AC0: void fn0000000140001AC0(Register word64 rax)
void fn0000000140001AC0(word64 rax)
{
	struct Eq_n * rax_n = DPB(rax, GetModuleHandleW(tLoc28), 0);
	if (rax_n == null || rax_n->w0000 != 23117)
		return;
	struct Eq_n * rax_n = rax_n + (int64) rax_n->dw003C / 0x0040;
	if (rax_n->dw0000 != 0x4550 || (rax_n->w0018 != 0x020B || rax_n->dw0084 <= 0x0E))
		;
}

// 0000000140001B14: void fn0000000140001B14(Stack Eq_n tArg00)
void fn0000000140001B14(Eq_n tArg00)
{
	SetUnhandledExceptionFilter(tArg00);
}

// 0000000140001B24: void fn0000000140001B24(Register (ptr64 (ptr64 Eq_n)) rcx, Register (ptr64 int32) rbx, Register word64 rsi, Register word64 rdi)
void fn0000000140001B24(struct Eq_n ** rcx, int32 * rbx, word64 rsi, word64 rdi)
{
	struct Eq_n * rax_n = *rcx;
	if (rax_n->dw0000 != ~0x1F928C9C || rax_n->dw0018 != 0x04)
		return;
	uint64 rcx_n = (uint64) rax_n->dw0020;
	word32 ecx_n = (word32) rcx_n;
	rcx_n = DPB(rcx_n, ecx_n, 0);
	struct Eq_n * rax_n = DPB(rax_n, rcx_n + ~0x1993051F, 0);
	if (rcx_n > ~0x19930521 && ecx_n != 0x01994000)
		return;
	api-ms-win-crt-runtime-l1-1-0.dll!terminate();
	int3();
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	// Failed to bind call argument.
	// Please report this issue at https://github.com/uxmal/reko
	Eq_n stackArg8 = <invalid>;
	Eq_n stackArg16 = <invalid>;
	fn0000000140001B5C(rbx, rsi, rdi, stackArg8, stackArg16);
}

// 0000000140001B5C: void fn0000000140001B5C(Register (ptr64 int32) rbx, Register word64 rsi, Register word64 rdi, Stack Eq_n qwArg08, Stack Eq_n qwArg10)
void fn0000000140001B5C(int32 * rbx, word64 rsi, word64 rdi, Eq_n qwArg08, Eq_n qwArg10)
{
	word64 * rbx_n;
	for (rbx_n = &globals->qw40002680; rbx_n < &globals->qw40002680; ++rbx_n)
	{
		<anonymous> * rdi_n = *rbx_n;
		if (rdi_n != null)
		{
			fn0000000140001BF4();
			rdi_n();
		}
	}
}

// 0000000140001BA8: void fn0000000140001BA8(Register word64 rbx, Register word64 rsi, Register word64 rdi, Stack word64 qwArg08, Stack word64 qwArg10)
void fn0000000140001BA8(word64 rbx, word64 rsi, word64 rdi, word64 qwArg08, word64 qwArg10)
{
	word64 * rbx_n;
	for (rbx_n = &globals->qw40002690; rbx_n < &globals->qw40002690; ++rbx_n)
	{
		<anonymous> * rdi_n = *rbx_n;
		if (rdi_n != null)
		{
			fn0000000140001BF4();
			rdi_n();
		}
	}
}

// 0000000140001BF4: void fn0000000140001BF4()
void fn0000000140001BF4()
{
	globals->ptr40002190();
}

// 0000000140001BFC: void fn0000000140001BFC(Register word32 edx, Register Eq_n ebx, Stack Eq_n qwArg08, Stack Eq_n qwArg10, Stack Eq_n qwArg18)
void fn0000000140001BFC(word32 edx, Eq_n ebx, Eq_n qwArg08, Eq_n qwArg10, Eq_n qwArg18)
{
	word32 ebx_n = (word32) rbx;
	globals->dw4000301C = 0x02;
	__cpuid(0x00, 0x00, &0x00, &ebx_n, &0x00, &edx);
	globals->dw40003018 = 0x01;
	rdx_n = (uint64) ((word32) (uint64) ebx_n ^ 1970169159);
	ui32 edx_n = (word32) rdx_n;
	ui32 r8d_n = (word32) (uint64) globals->dw40003614;
	__cpuid(0x01, 0x00, &0x01, &ebx_n, &0x00, &edx_n);
	byte bLoc20_n = SLICE(dwLoc20 & 0x00, byte, 0);
	word32 r11d_n = (word32) (uint64) ((word32) (uint64) ((word32) (uint64) ((word32) (uint64) ebx_n ^ 1752462657) | (word32) ((uint64) ((word32) ((uint64) edx) ^ 0x69746E65))) | 0x444D4163);
	uint64 rdx_n = DPB(rdx_n, edx_n, 0);
	rdx_n = DPB(rdx_n, edx_n, 0);
	rdx_n = DPB(rdx_n, edx_n, 0);
	rdx_n = DPB(rdx_n, edx_n, 0);
	rdx_n = DPB(rdx_n, edx_n, 0);
	rdx_n = DPB(rdx_n, edx_n, 0);
	uint64 rdx_n = DPB(rdx_n, edx_n, 0);
	rdx_n = DPB(rdx_n, edx_n, 0);
	rdx_n = DPB(rdx_n, edx_n, 0);
	uint64 rdx_n = DPB(rdx_n, edx_n, 0);
	rdx_n = DPB(rdx_n, edx_n, 0);
	if ((word32) (uint64) ((word32) (uint64) ((word32) (uint64) ((word32) (uint64) edx ^ 0x49656E69) | 1818588270) | edx_n) == 0x00)
	{
		globals->qw40003020 |= ~0x00;
		r8d_n = (word32) (uint64) (r8d_n | 0x04);
		globals->dw40003614 = r8d_n;
		if (~0x000106BF == 0x00 || (~0x0002065F == 0x00 || ~0x0002066F == 0x00))
		{
l0000000140001CDE:
			r8d_n = (word32) (uint64) (r8d_n | 0x01);
			globals->dw40003614 = r8d_n;
			goto l0000000140001CE9;
		}
		if (true)
		{
			ebx_n = 0x00010001;
			if (!__bt(0x00010001, 0xFFFCF9B0))
				goto l0000000140001CDE;
		}
	}
l0000000140001CE9:
	if (r11d_n == 0x00 && true)
	{
		r8d_n = (word32) (uint64) (r8d_n | 0x04);
		globals->dw40003614 = r8d_n;
	}
	if (false)
	{
		__cpuid(0x07, 0x00, &0x07, &ebx_n, &0x00, &edx_n);
		bLoc20_n = SLICE(ebx_n, byte, 0);
		if (!__bt(ebx_n, 0x09))
			globals->dw40003614 = (word32) (uint64) (r8d_n | 0x02);
	}
	if (!__bt(0x00, 0x14))
	{
		globals->dw40003018 = 0x02;
		globals->dw4000301C = 0x06;
		if (!__bt(0x00, 0x1B) && !__bt(0x00, 0x1C))
		{
			word64 edx_eax_n = __xgetbv(0x00);
			if (((byte) (DPB(rdx_n, edx_eax_n, 0) << 0x20 | DPB(0x07, edx_eax_n, 0)) & 0x06) == 0x06)
			{
				ui32 eax_n = (word32) (uint64) ((word32) (uint64) globals->dw4000301C | 0x08);
				globals->dw40003018 = 0x03;
				globals->dw4000301C = eax_n;
				if ((bLoc20_n & 0x20) != 0x00)
				{
					globals->dw40003018 = 0x05;
					globals->dw4000301C = (word32) (uint64) (eax_n | 0x20);
				}
			}
		}
	}
}

// 0000000140001DC4: Register uint64 fn0000000140001DC4()
uint64 fn0000000140001DC4()
{
	return (uint64) (globals->dw40003030 != 0x00);
}

// 0000000140001DD0: void fn0000000140001DD0()
void fn0000000140001DD0()
{
}

// 0000000140001E7C: void fn0000000140001E7C(Register int64 rdx, Register word64 rbx, Register word64 rsi, Register word64 rdi, Register (ptr64 Eq_n) r9)
void fn0000000140001E7C(int64 rdx, word64 rbx, word64 rsi, word64 rdi, struct Eq_n * r9)
{
	fn0000000140001E9C(rdx, r9, rbx, rsi, rdi, r9->ptr0038, qwLoc30);
}

// 0000000140001E9C: void fn0000000140001E9C(Register int64 rcx, Register (ptr64 Eq_n) rdx, Register word64 rbx, Register word64 rsi, Register word64 rdi, Register (ptr64 Eq_n) r8, Stack word64 qwArg00)
void fn0000000140001E9C(int64 rcx, struct Eq_n * rdx, word64 rbx, word64 rsi, word64 rdi, struct Eq_n * r8, word64 qwArg00)
{
	word32 r11d_n = (word32) (uint64) ((word32) (uint64) r8->dw0000 & ~0x07);
	int64 r9_n = rcx;
	int64 r10_n = rcx;
	if ((r8->dw0000 & 0x04) != 0x00)
		r10_n = (int64) r8->dw0004 + rcx & (int64) ((word32) ((uint64) (-((word32) ((uint64) r8->dw0008)))));
	word64 rdx_n = (int64) r11d_n + r10_n;
	struct Eq_n * rcx_n = (uint64) rdx->ptr0010->dw0008 + rdx->qw0008;
	if ((rcx_n->b0003 & 0x0F) != 0x00)
		r9_n = rcx + (uint64) ((word32) rcx_n->b0003 & ~0x0F);
	Eq_n r9_n = r9_n ^ rdx_n;
	fn00000001400011B0(r9_n, rbx, rsi, rdi, qwArg00, out rcx_n, out rdx_n, out r8_n);
}

// 0000000140001F10: void fn0000000140001F10(Register (ptr64 code) rax)
void fn0000000140001F10(<anonymous> * rax)
{
	rax();
}

// 0000000140001F12: void fn0000000140001F12(Register (ptr64 word64) rcx, Register word64 rdx, Register word64 rbp)
void fn0000000140001F12(word64 * rcx, word64 rdx, word64 rbp)
{
	word64 rax_n = *rcx;
	seh_filter_exe();
}

// 0000000140001F30: void fn0000000140001F30(Register word64 rcx)
void fn0000000140001F30(word64 rcx)
{
}

