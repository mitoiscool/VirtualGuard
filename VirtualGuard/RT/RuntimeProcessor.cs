using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.RT;

public class RuntimeProcessor
{
    
    public RuntimeProcessor(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        _runtime = rt;
        _ctx = ctx;
    }
    
    private VirtualGuardRT _runtime;
    private VirtualGuardContext _ctx;

    public void FinalizeMethods()
    {
        foreach (var virtualizedMethodKvp in _ctx.VirtualizedMethods)
        {
            virtualizedMethodKvp.Deconstruct(out MethodDefinition meth, out bool export);
            
            if(export)
                PatchMethod(meth);
            else
                RemoveMethod(meth);
            
            
        }
        
        
        
    }


    void PatchMethod(MethodDefinition meth)
    {
        var loc = _runtime.GetExportLocation(meth);

        var instrs = meth.CilMethodBody.Instructions;
        
        
        instrs.Clear();
        
        // VMEntry(loc, args)
        
        instrs.Add(CilOpCodes.Ldc_I4, loc);
        
        instrs.Add(CilOpCodes.Ldnull);
        
        instrs.Add(CilOpCodes.Call, _runtime.Elements.VmEntry);
        
        if(!meth.Signature.ReturnsValue)
            instrs.Add(CilOpCodes.Pop);
        
        instrs.Add(CilOpCodes.Ret);
        
        meth.CilMethodBody.VerifyLabels();
        meth.CilMethodBody.ComputeMaxStack();
    }

    void RemoveMethod(MethodDefinition def)
    {
        
    }
    
    
}