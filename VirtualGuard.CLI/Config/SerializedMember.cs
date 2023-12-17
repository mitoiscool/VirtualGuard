using AsmResolver.DotNet;

namespace VirtualGuard.CLI.Config;

[Serializable]
public class SerializedMember
{
    public string Member;

    public bool Virtualize;
    public bool Exclude;

    public IMemberDefinition Resolve(ModuleDefinition mod)
    {
        if (!Member.Contains(":")) // is type
            return mod.LookupType(Member);

        return mod.LookupMember(Member);
    }
    
}