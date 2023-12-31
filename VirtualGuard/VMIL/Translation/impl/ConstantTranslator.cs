using System.Reflection;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class ConstantTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Ldc_I4:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand));
                break;
            
            case CilCode.Ldc_I8:
                block.WithContent(new VmInstruction(VmCode.Ldc_I8, instr.Operand));
                break;
            
            case CilCode.Ldc_R4:
                block.WithContent(new VmInstruction(VmCode.Ldc_R4, instr.Operand));
                break;
            
            case CilCode.Ldc_R8:
                block.WithContent(new VmInstruction(VmCode.Ldc_R8, instr.Operand));
                break;
            
            case CilCode.Ldnull:
                block.WithContent(new VmInstruction(VmCode.Ldstr, 0)); // loads null
                break;
            
            case CilCode.Ldstr:
                block.WithContent(new VmInstruction(VmCode.Ldstr, instr.Operand)); // encoded later on
                break;
        }
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        return new []
        {
            CilCode.Ldc_I4,
            CilCode.Ldc_I8,
            
            CilCode.Ldc_R4,
            CilCode.Ldc_R8,
            
            CilCode.Ldstr,
            CilCode.Ldnull,
        }.Contains(instr.OpCode.Code);
    }
}