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

        clonedMethod.Name = new Random().Next().ToString("x");
        
        // inject cloned method
        ctx.Module.ManagedEntryPointMethod.DeclaringType.Methods.Add(clonedMethod);
        
        // mark for virtualization
        ctx.Virtualizer.AddMethod(clonedMethod, true);
        
        var entry = ctx.Module.ManagedEntryPointMethod;

        entry.CilMethodBody.Instructions.Insert(0, CilOpCodes.Call, clonedMethod);
    }
}