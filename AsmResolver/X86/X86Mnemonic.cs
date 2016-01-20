﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmResolver.X86
{
    /// <summary>
    /// Provides valid mnemonics of an x86 instruction.
    /// </summary>
    public enum X86Mnemonic : ushort
    {
        Unknown,
        Add,
        Push,
        Pop,
        Or,
        Adc,
        Sbb,
        And,
        Sub,
        Daa,
        Das,
        Xor,
        Cmp,
        Aaa,
        Aas,
        Inc,
        Dec,
        Pushad,
        Popad,
        Bound,
        Arpl,
        Ins,
        Outs,
        Jo,
        Jno,
        Jb,
        Jnb,
        Je,
        Jne,
        Jbe,
        Ja,
        Js,
        Jns,
        Jpe,
        Jpo,
        Jl,
        Jge,
        Jle,
        Jg,
        Test,
        Xchg,
        Mov,
        Lea,
        Nop,
        Cwde,
        Cdq,
        Call_Far,
        Wait,
        Pushfd,
        Popfd,
        Sahf,
        Lahf,
        Movsb,
        Movsd,
        Cmpsb,
        Cmpsd,
        Stosb,
        Stosd,
        Lodsb,
        Lodsd,
        Scasb,
        Scasd,
        Rol,
        Ror,
        Rcl,
        Rcr,
        Shl,
        Shr,
        Sal,
        Sar,
        Retn,
        Les,
        Lds,
        Enter,
        Leave,
        Retf,
        Int3,
        Int,
        Into,
        Iretd,
        Amx,
        Adx,
        Salc,
        Xlatb,
        Loopne,
        Loope,
        Loop,
        Jecxz,
        In,
        Out,
        Call,
        Jmp,
        Jmp_Far,
        Lock,
        Int1,
        Hlt,
        Cmc,
        Not,
        Neg,
        Mul,
        Imul,
        Div,
        Idiv,
        Clc,
        Stc,
        Cli,
        Sti,
        Cld,
        Std,
        Rep
    }
}
