using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.RT.Chunk;

namespace VirtualGuard.VMIL.VM;

internal class DynamicStartKeyReference
{
    public DynamicStartKeyReference(ControlFlowNode<CilInstruction> node, bool isConditional)
    {
        Node = node;
        IsConditional = isConditional;
    }

    public bool IsConditional;
    
    public VmBlock VmBlock;
    public VmChunk Chunk;
    public ControlFlowNode<CilInstruction> Node;
}