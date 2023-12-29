using AsmResolver.DotNet;

namespace VirtualGuard.RT;

public struct VmElements
{
    public MethodDefinition VmEntry;
    public MethodDefinition VmEntryInst;

    public TypeDefinition[] VmTypes;
}