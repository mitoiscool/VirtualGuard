using AsmResolver.DotNet;
using AsmResolver.DotNet.Cloning;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.CLI.Processors.impl;

public class FreeLimitations : IProcessor
{
    public string Identifier => "Free Limitations";

    public void Process(Context ctx)
    {
        // inject limiter stub, reference in random virtualized methods
        
        // thought is we just drop a simple console.writeline and i'd need to implement virtual fields but yea
        
        // now to remember how to use member cloner

        var cloner = new MemberCloner(ctx.Module);

        cloner.Include(ctx.LocateStub("Limit"));

        var res = cloner.Clone();

        var clonedMethod = (MethodDefinition)res.ClonedMembers.Single(x => x.Name == "Limit");
        
        // inject cloned method
        ctx.Module.GetOrCreateModuleType().Methods.Add(clonedMethod);
        
        // mark for virtualization
        ctx.Virtualizer.AddMethod(clonedMethod, true);
        
        // find entrypoint
        var cctor = ctx.Module.GetOrCreateModuleConstructor();

        if (cctor.CilMethodBody == null)
            cctor.CilMethodBody = new CilMethodBody(cctor);

        cctor.CilMethodBody.Instructions.Add(CilOpCodes.Call, clonedMethod);
        cctor.CilMethodBody.Instructions.Add(CilOpCodes.Ret);
    }
    
}