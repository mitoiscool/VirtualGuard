using AsmResolver.DotNet;
using VirtualGuard.CLI.Config;
using VirtualGuard.RT;

namespace VirtualGuard.CLI;

public class Context
{
    public Context(ModuleDefinition mod, SerializedConfig cfg)
    {
        Module = mod;
        Configuration = cfg;
    }
    
    public ModuleDefinition Module;
    public Virtualizer Virtualizer;
    public SerializedConfig Configuration;

    public VmElements GetVm() => Virtualizer.GetVmElements();
}