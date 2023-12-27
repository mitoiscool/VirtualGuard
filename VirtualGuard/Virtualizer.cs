using System.Diagnostics;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Cloning;
using VirtualGuard.RT;
using VirtualGuard.RT.Chunk;

namespace VirtualGuard;

public class Virtualizer
{
    private VirtualGuardContext _ctx;

    private MethodVirtualizer _methodVirtualizer;
    private VirtualGuardRT _rt;

    private bool _rtCommitted = false;
    
    public Virtualizer(VirtualGuardContext ctx, int debugKey, bool debug)
    {
        _ctx = ctx;
        _rt = new VirtualGuardRT(ModuleDefinition.FromFile(RuntimeConfig.RuntimePath), debugKey, debug);
        _methodVirtualizer = new MethodVirtualizer(_rt, _ctx);
        _ctx.Runtime = _rt;
    }

    public async Task AddMethod(MethodDefinition def, bool exportMethod)
    {
        var sw = new Stopwatch();
        sw.Start();
        
        Console.WriteLine("Virtualizing " + def.Name);
        
        _methodVirtualizer.Virtualize(def, exportMethod);
        _ctx.VirtualizedMethods.Add(def, exportMethod);
        
        sw.Stop();
        
        Console.WriteLine("Finished: " + sw.ElapsedMilliseconds + "ms");
    }

    public void CommitRuntime()
    {
        _rt.BuildData(_ctx);

        // clone runtime module into target module
        _rt.Inject(_ctx.Module);

        var processor = new RuntimeProcessor(_rt, _ctx);
        processor.FinalizeMethods();

        _rtCommitted = true;
    }

    public VmElements GetVmElements()
    {
        if(!_rtCommitted)
            _ctx.Logger.Fatal("Runtime has not been committed before requesting VmElements.");
            
        return _rt.Elements;
    }
    
    
}