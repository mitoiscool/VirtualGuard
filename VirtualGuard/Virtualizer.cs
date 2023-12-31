using System.Diagnostics;
using System.Reflection;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Cloning;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.RT;
using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.Translation;

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

    [Obfuscation(Feature = "virtualization")]
    public void AddMethod(MethodDefinition def, bool exportMethod)
    {
        var sw = new Stopwatch();
        sw.Start();

        _ctx.Logger.Info("Virtualizing " + def.FullName);
        
        _methodVirtualizer.Virtualize(def, exportMethod);
        _ctx.VirtualizedMethods.Add(def, exportMethod);
        
        sw.Stop();
        
        Console.WriteLine("Finished in: " + sw.ElapsedMilliseconds + "ms");
    }

    [Obfuscation(Feature = "virtualization")]
    public void CommitRuntime()
    {
        if (_ctx.VirtualizedMethods.Count == 0)
        {
            _ctx.Logger.Warning("Virtualizer had no methods virtualized, not injecting.");
            return;
        }

        _rt.BuildData(_ctx);

        // clone runtime module into target module
        _rt.Inject(_ctx.Module);

        var processor = new RuntimeProcessor(_rt, _ctx);
        processor.FinalizeMethods();

        _rtCommitted = true;
    }

    [Obfuscation(Feature = "virtualization")]
    public VmElements GetVmElements()
    {
        if (_ctx.VirtualizedMethods.Count == 0)
            return new VmElements()
            {
                VmTypes = Array.Empty<TypeDefinition>()
            }; // kinda scuffed but in the present usages it's fine
        
        if(!_rtCommitted)
            _ctx.Logger.Fatal("Runtime has not been committed before requesting VmElements.");
            
        return _rt.Elements;
    }

    public static bool Supports(CilOpCode code)
    {
        var newExpr = new AstExpression(code, null, null);

        if (ITranslator.Lookup(newExpr) == null)
            return false;

        return true; // has a translator, doesn't necessarily mean it'll work ;)
    }
    
}