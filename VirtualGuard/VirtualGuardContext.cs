using AsmResolver.DotNet;
using VirtualGuard.RT;

namespace VirtualGuard;

public class VirtualGuardContext
{
    public VirtualGuardContext(ModuleDefinition mod, ILogger logger)
    {
        Module = mod;
        Logger = logger;
    }

    internal VirtualGuardRT Runtime;
    public ILogger Logger;
    public ModuleDefinition Module;
    public Dictionary<MethodDefinition, bool> VirtualizedMethods = new Dictionary<MethodDefinition, bool>(); // populated once methods added
    
}