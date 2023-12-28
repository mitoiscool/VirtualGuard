using AsmResolver.DotNet;

namespace VirtualGuard.RT.Mutators.impl;

public class TokenAllocator : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        var importedMembers = new List<MetadataMember>();
        
        foreach (var member in rt.VmChunks.SelectMany(x => x.Content).Where(x => x.Operand is MetadataMember).Select(x => x.Operand).Cast<MetadataMember>().Where(x => x.MetadataToken.Rid == 0))
        { // unassigned
            if(importedMembers.Contains(member))
                continue;

            ctx.Module.TokenAllocator.AssignNextAvailableToken(member);
            importedMembers.Add(member);
        }
    }
}