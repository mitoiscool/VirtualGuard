using AsmResolver.DotNet;

namespace VirtualGuard;

public class VirtualGuardContext
{
    public VirtualGuardContext(ModuleDefinition mod, ILogger logger)
    {
        Module = mod;
        Logger = logger;
    }

    public ILogger Logger;
    public ModuleDefinition Module;
    public Dictionary<MethodDefinition, bool> VirtualizedMethods = new Dictionary<MethodDefinition, bool>(); // populated once methods added

}