using System.Reflection;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class FieldTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Stfld:
            case CilCode.Stsfld:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand));
                block.WithContent(new VmInstruction(VmCode.Stfld));
                break;
            
            case CilCode.Ldfld:
            case CilCode.Ldsfld:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand));
                block.WithContent(new VmInstruction(VmCode.Ldfld));
                break;
            
            case CilCode.Ldflda:
            case CilCode.Ldsflda:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand));
                block.WithContent(new VmInstruction(VmCode.Ldflda));
                break;
        }
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Ldfld,
            CilCode.Ldsfld,
            CilCode.Ldflda,
            CilCode.Ldsflda,

            CilCode.Stfld,
            CilCode.Stsfld
        }.Contains(instr.OpCode.Code);
    }
}