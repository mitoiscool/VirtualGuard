namespace VirtualGuard.VMIL.VM;

public enum VmCode
{
    Add,
    Sub,
    Mul,
    Div,
    
    Stloc,
    Ldloc,
    
    Ldc_I4,
    Ldc_I8,
    
    Pop,
    Ret,
    
    LoadState,
    
    Cmp,
    
    Dup,
    
    __ldelem, // these will be vcalls in special region
    __stelem, // these will be vcalls
    
    __jmploc, // change to ldc_i4 on encode time, ensure we get the latest offset
    __jmp,
    __jz,
    __jnz,
}