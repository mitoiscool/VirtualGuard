using System.Diagnostics;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.Handlers;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class MarkerTranslator : ITranslator
{
    private Stack<UnknownBlockLink> _exceptionHandlers = new Stack<UnknownBlockLink>();
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        int i = 0;
        var marker = instr as Marker;

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

    public bool Supports(CilInstruction instr)
    {
        if (instr is Marker)
            return true;

        return false;
    }
}

public class UnknownBlockLink
{
    public UnknownBlockLink(int id)
    {
        LinkIdentifier = id;
    }
    
    public int LinkIdentifier;

    public bool Linked => LinkedBlock != null;

    public VmBlock LinkedBlock;
}