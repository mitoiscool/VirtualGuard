using AsmResolver.DotNet;

namespace VirtualGuard.CLI.Config;

[Serializable]
public class SerializedConfig
{
    public SerializedMember[] Members;
    
    public bool RenameDebugSymbols;
    public bool UseDataEncryption;
    
    
    public MethodDefinition[] ResolveVirtualizedMethods(ModuleDefinition mod)
    {
        return Members.Where(x => x is { Virtualize: true, Exclude: false }).Select(x => x.Resolve(mod)).Cast<MethodDefinition>().ToArray();
    }

    public bool IsMemberExcluded(IMemberDefinition def, ModuleDefinition mod)
    {
        return Members.Where(x => x.Exclude).Select(x => x.Resolve(mod)).Contains(def);
    }
    
    
    
}