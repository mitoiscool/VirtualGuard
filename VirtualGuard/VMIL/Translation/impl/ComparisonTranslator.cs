using System.Diagnostics;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ComparisonTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        // handle hashing of args
        
        
        if(instr.OpCode.Code == CilCode.Ceq)
            block.WithContent(Util.BuildHashInstructions(instr, meth, ctx.Runtime)); // see about hashing for equals operations
        
        // now we just need to push the branch condition, aka validating the output flag
        
        block.WithContent(new VmInstruction(VmCode.Cmp)); // main cmp
        
        switch (instr.OpCode.Code)
        {
            case CilCode.Ceq:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, (int)meth.Runtime.Descriptor.ComparisonFlags.EqFlag));
                break;
            
            case CilCode.Cgt:
            case CilCode.Cgt_Un:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, (int)meth.Runtime.Descriptor.ComparisonFlags.GtFlag));
                break;
            
            case CilCode.Clt:
            case CilCode.Clt_Un:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, (int)meth.Runtime.Descriptor.ComparisonFlags.LtFlag));
                break;
        }
        
        // now we need to determine if the values are the same to produce the 1 or 0 flag
        
        block.WithContent(new VmInstruction(VmCode.Xor)); // this will push 0 if they are equal
    }

    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Ceq,
            CilCode.Cgt,
            CilCode.Cgt_Un,
            CilCode.Clt,
            CilCode.Clt_Un,
        }.Contains(instr.OpCode.Code);
    }
    
}