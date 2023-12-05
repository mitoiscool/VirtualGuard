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
                RemoveMethod();
            
            
        }
        
        
        
    }


    void PatchMethod(MethodDefinition meth)
    {
        var loc = _runtime.GetExportLocation(meth);

        var instrs = meth.CilMethodBody.Instructions;
        
        
        instrs.Clear();

        instrs.Add(CilOpCodes.Ldc_I4, loc);
        instrs.Add(CilOpCodes.Ret);
    }

    void RemoveMethod()
    {
        
    }
    
    
}