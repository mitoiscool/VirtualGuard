using System.Reflection.Emit;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ConditionalTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {

        block.WithContent(new VmInstruction(VmCode.Ldc_I4, instr.Operand));
        
        switch (instr.OpCode.Code)
        {
            case CilCode.Brfalse:
                block.WithContent(new VmInstruction(VmCode.Jnz));
                break;
            
            case CilCode.Brtrue:
                block.WithContent(new VmInstruction(VmCode.Jz));
                break;
        }
        //if (instr.OpCode.Code == CilCode.Brfalse) // my brain still cannot wrap my head around why this works this way, all signs pooint to not being used for brtrue, but debugging prevails
            //block.WithContent(new VmInstruction(VmCode.Not));

            // yea cause it didn't work goof lol
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