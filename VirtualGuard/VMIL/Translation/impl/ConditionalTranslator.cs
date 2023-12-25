using System.Reflection.Emit;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ConditionalTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {

        //if (instr.OpCode.Code == CilCode.Brfalse) // my brain still cannot wrap my head around why this works this way, all signs pooint to not being used for brtrue, but debugging prevails
            //block.WithContent(new VmInstruction(VmCode.Not));

        block.WithContent(
            new VmInstruction(VmCode.Ldc_I4, instr.Operand),
            new VmInstruction(VmCode.Jz));
    }

    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Brtrue,
            CilCode.Brfalse
        }.Contains(instr.OpCode.Code);
    }
}