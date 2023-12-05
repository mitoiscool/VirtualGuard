using System.Diagnostics;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Cloning;
using VirtualGuard.RT;

namespace VirtualGuard;

public class Virtualizer
{
    private VirtualGuardContext _ctx;

    private MethodVirtualizer _methodVirtualizer;
    private VirtualGuardRT _rt;
    
    public Virtualizer(VirtualGuardContext ctx)
    {
        _ctx = ctx;
        _rt = new VirtualGuardRT(ModuleDefinition.FromFile(RuntimeConfig.RuntimePath));
        _methodVirtualizer = new MethodVirtualizer(_rt);
    }

    public void AddMethod(MethodDefinition def, bool exportMethod)
    {
        var sw = new Stopwatch();
        sw.Start();
        
        Console.WriteLine("Virtualizing " + def.Name);
        
        _methodVirtualizer.Virtualize(def, exportMethod);
        
        sw.Stop();
        
        Console.WriteLine("Finished: " + sw.ElapsedMilliseconds + "ms");
    }

    public void CommitRuntime()
    {
        
        // clone runtime module into target module
        var cloner = new MemberCloner(_ctx.Module);

        var members = _rt.RuntimeModule.GetAllTypes().ToArray();

        cloner.Include(members);

        var resut = cloner.Clone();
    }
    
    
    
    
    
}