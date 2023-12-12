using AsmResolver.DotNet;

namespace VirtualGuard;

public class VirtualGuardContext
{
    public static VirtualGuardContext Load(string path, VirtualGuardSettings settings)
    {
        return new VirtualGuardContext(ModuleDefinition.FromFile(path), settings);
    }


    public VirtualGuardContext(ModuleDefinition mod, VirtualGuardSettings settings)
    {
        Module = mod;
        Settings = settings;
    }

    public ModuleDefinition Module;
    public VirtualGuardSettings Settings;
    public Dictionary<MethodDefinition, bool> VirtualizedMethods = new Dictionary<MethodDefinition, bool>(); // populated once methods added

}