using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class RetTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        block.WithContent(
            new VmInstruction(VmCode.Ret));
    }

    public bool Supports(AstExpression instr)
    {
        return instr.OpCode.Code == CilCode.Ret;
    }
}