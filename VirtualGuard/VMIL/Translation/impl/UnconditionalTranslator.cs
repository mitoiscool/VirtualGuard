using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class UnconditionalTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        block.WithContent(
            new VmInstruction(
                VmCode.Ldc_I4,
                instr.Operand),
            new VmInstruction(
                VmCode.Jmp
                )
            );
    }

    public bool Supports(AstExpression instr)
    {
        return instr.OpCode.Code == CilCode.Br;
    }
}