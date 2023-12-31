using System.Diagnostics;
using System.Reflection;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.AST.IL;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class MarkerTranslator : ITranslator
{
    private Stack<UnknownBlockLink> _exceptionHandlers = new Stack<UnknownBlockLink>();
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        int i = 0;
        var marker = instr as AstMarker;

        switch (marker.Type)
        {
            case MarkerType.TryStart:
                var unknownLink = new UnknownBlockLink(i++);
                _exceptionHandlers.Push(unknownLink);
                block.WithContent(
                    new VmInstruction(VmCode.Ldc_I4, unknownLink),
                    new VmInstruction(VmCode.Entertry));
                break;

            case MarkerType.HandlerStart: // we can resolve the block
                var reference = _exceptionHandlers.Pop();
                reference.LinkedBlock = block; // link block
                break;
            
            
        }
        
        
        
        
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        if (instr is AstMarker)
            return true;

        return false;
    }
}

internal class UnknownBlockLink
{
    public UnknownBlockLink(int id)
    {
        LinkIdentifier = id;
    }
    
    public int LinkIdentifier;

    public bool Linked => LinkedBlock != null;

    public VmBlock LinkedBlock;
}