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
    public MultiProcessorVirtualizer Virtualizer;
    public SerializedConfig Configuration;

    public LicenseType License;
    
}