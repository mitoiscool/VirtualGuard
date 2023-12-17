using AsmResolver.DotNet;

namespace VirtualGuard.CLI.Config;

[Serializable]
public class SerializedConfig
{
    public SerializedMember[] Members;
    
    public bool RenameDebugSymbols;
    public bool UseDataEncryption;
    
    
    public (MethodDefinition, bool)[] ResolveVirtualizedMethods(Context ctx)
    {
        return Members
            .Where(x => x is { Virtualize: true, Exclude: false })
            .Select(x => x.Resolve(ctx))
            .Cast<MethodDefinition>()
            .Zip(
                Members
                    .Where(x => x is { Virtualize: true, Exclude: false })
                    .Select(x => x.VirtualInlining)
                    .Cast<bool>(),
                (method, shouldInline) => (method, shouldInline)
            )
            .ToArray();
    }

    public bool IsMemberExcluded(IMemberDefinition def, Context ctx)
    {
        return Members.Where(x => x.Exclude).Select(x => x.Resolve(ctx)).Contains(def);
    }
    
    public bool IsMemberVirtualized(IMemberDefinition def, Context ctx)
    {
        return Members.Where(x => x.Virtualize).Select(x => x.Resolve(ctx)).Contains(def);
    }
    
    
}