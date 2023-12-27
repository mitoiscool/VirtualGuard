using System.Reflection;
using AsmResolver.DotNet;
using VirtualGuard.CLI.Config;
using VirtualGuard.RT;
using VirtualGuard.Stubs;

namespace VirtualGuard.CLI;

public class Context
{
    public Context(ModuleDefinition mod, SerializedConfig cfg, ILogger logger, LicenseType license)
    {
        Module = mod;
        Configuration = cfg;
        Logger = logger;
        License = license;

        RuntimeModule = ModuleDefinition.FromFile(typeof(Limiter).Assembly.Location);
    }

    public ILogger Logger;
    public ModuleDefinition Module;
    public MultiProcessorVirtualizer Virtualizer;
    public SerializedConfig Configuration;

    public ModuleDefinition RuntimeModule;

    public LicenseType License;
    
    public MethodDefinition LocateStub(string name)
    {
        // get this module
        return RuntimeModule.GetAllTypes().SelectMany(x => x.Methods).Single(x => x.Name == name);
    }
    
    
    
}