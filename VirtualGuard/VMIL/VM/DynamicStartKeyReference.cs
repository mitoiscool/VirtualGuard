using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.RT.Chunk;

namespace VirtualGuard.VMIL.VM;

public class DynamicStartKeyReference
{
    public DynamicStartKeyReference(ControlFlowNode<CilInstruction> node)
    {
        Node = node;
    }

    public VmBlock VmBlock;
    public VmChunk Chunk;
    public ControlFlowNode<CilInstruction> Node;
}