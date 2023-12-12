namespace VirtualGuard.VMIL.VM;

public enum VmCode : byte
{
    Add,
    Sub,
    Mul,
    Div,
    
    Not,
    Rem,
    Xor,
    And,
    Or,
    
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
    
    Jmp,
    Jz,
    Jnz,
    
    Dup,
    
    Ldelem, // these will be vcalls in special region
    Ldelema,
    
    Stelem, // these will be vcalls

    Vmcall // can be a sick ass vcall, jmp to region, push all args and relativize ctx
}