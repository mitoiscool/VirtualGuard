using System.Diagnostics;
using System.Reflection;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class ExceptionTranslator : ITranslator
{
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Leave:
            case CilCode.Leave_S:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand),
                    new VmInstruction(VmCode.Leave, new DynamicStartKeyReference((ControlFlowNode<CilInstruction>)instr.Operand, false))
                    );
                break;
        }
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Leave,
            CilCode.Leave_S
        }.Contains(instr.OpCode.Code);
    }
}