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
        if (ctx.ResolutionCache.TryGetValue(this, out IMemberDefinition def))
            return def;
        
        try
        {
            if (!Member.Contains(":"))
            {
                var type = ctx.Module.LookupType(Member);

                ctx.ResolutionCache.Add(this, type);
                return type;
            }

            var member = ctx.Module.LookupMember(Member);
            
            ctx.ResolutionCache.Add(this, member);
            return member;
        }
        catch
        {
            ctx.Logger.Fatal("Could not resolve member " + Member);
            return null;
        }
    }
    
}