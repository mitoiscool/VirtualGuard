using AsmResolver.DotNet;

namespace VirtualGuard;

public class VirtualGuardContext
{
    public static VirtualGuardContext Load(string path, VirtualGuardSettings settings)
    {
        return new VirtualGuardContext(ModuleDefinition.FromFile(path));
    }


    public VirtualGuardContext(ModuleDefinition mod)
    {
        Module = mod;
    }

    public ModuleDefinition Module;
    public VirtualGuardSettings Settings;
    
}