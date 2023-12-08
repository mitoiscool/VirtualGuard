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
    
    Stfld,
    Ldfld,
    
    LoadState,
    EnterRegion,
    
    Cmp,
    
    Jmp,
    
    Dup,
    
    __ldelem, // these will be vcalls in special region
    __stelem, // these will be vcalls

    __jz,
    __jnz,
}