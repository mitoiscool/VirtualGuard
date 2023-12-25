using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class MemberOpTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Ldtoken:
                block.WithContent(new VmInstruction(VmCode.Ldtoken, instr.Operand));
                break;
        }
    }

    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Ldtoken
        }.Contains(instr.OpCode.Code);
        
    }
}