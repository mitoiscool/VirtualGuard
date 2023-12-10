namespace VirtualGuard.VMIL.VM;

public enum VmCode
{
    Add,
    Sub,
    Mul,
    Div,
    
    Not,
    
    Stloc,
    Ldloc,
    
    Ldc_I4,
    Ldc_I8,
        
    Ldstr,
    
    Ldc_R4,
    Ldc_R8,
    
    Call,
    
    Pop,
    Ret,
    
    Stfld,
    Ldfld,
    
    LoadState,
    EnterRegion,
    
    Cmp,
    
    Jmp,
    
    Dup,
    Ldelem, // these will be vcalls in special region
    Stelem, // these will be vcalls

    Jz,
}