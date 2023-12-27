using AsmResolver.DotNet;

namespace VirtualGuard.CLI.Config;

[Serializable]
public class SerializedMember
{
    public string Member;

    public bool Virtualize;
    public bool VirtualInlining;
    public bool Exclude;

    public IMemberDefinition Resolve(Context ctx)
    {
        try
        {
            if (!Member.Contains(":")) // is type
                return ctx.Module.LookupType(Member);

            return ctx.Module.LookupMember(Member);
        }
        catch
        {
            ctx.Logger.Fatal("Could not resolve member " + Member);
            return null;
        }
    }
    
}