using AsmResolver.DotNet;
using VirtualGuard.CLI.Config;
using VirtualGuard.RT;

namespace VirtualGuard.CLI;

public class Context
{
    public Context(ModuleDefinition mod, SerializedConfig cfg, ILogger logger)
    {
        Module = mod;
        Configuration = cfg;
        Logger = logger;
    }

    public ILogger Logger;
    public ModuleDefinition Module;
    public Virtualizer Virtualizer;
    public SerializedConfig Configuration;

    public LicenseType License;

    public VmElements GetVm() => Virtualizer.GetVmElements();
}