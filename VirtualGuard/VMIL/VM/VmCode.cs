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
    Ldloca,
    
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
    Ldflda,
    
    LoadState,
    EnterRegion,
    
    Cmp,
    FlagJmp,
    
    Jmp,
    
    Dup,
    
    Ldelem, // these will be vcalls in special region
    Stelem, // these will be vcalls

    Jz,
    Vmcall // can be a sick ass vcall, jmp to region, push all args and relativize ctx
}