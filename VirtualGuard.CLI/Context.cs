using AsmResolver.DotNet;
using VirtualGuard.CLI.Config;
using VirtualGuard.RT;

namespace VirtualGuard.CLI;

public class Context
{
    public Context(ModuleDefinition mod, SerializedConfig cfg, ILogger logger, LicenseType license)
    {
        Module = mod;
        Configuration = cfg;
        Logger = logger;
        License = license;
    }

    public ILogger Logger;
    public ModuleDefinition Module;
    public Virtualizer[] Virtualizers;
    public SerializedConfig Configuration;

    public LicenseType License;
    
    private static Random _rnd = new Random();

    public void MarkForVirtualization(MethodDefinition def, bool export)
    {
        var vm = Virtualizers[_rnd.Next(Virtualizers.Length)];

        vm.AddMethod(def, export);
    }

    public void CommitProcessors()
    {
        foreach (var virt in Virtualizers)
        {
            virt.CommitRuntime();
        }
    }
    
    
}