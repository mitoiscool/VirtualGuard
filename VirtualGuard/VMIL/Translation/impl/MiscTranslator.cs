using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class MiscTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        switch (instr.OpCode.Code)
        {
            case CilCode.Dup:
                block.WithContent(new VmInstruction(VmCode.Dup));
                break;
            
            case CilCode.Pop:
                block.WithContent(new VmInstruction(VmCode.Pop));
                break;
        }
        
    }

    public bool Supports(CilInstruction instr)
    {
        return instr.OpCode.Code == CilCode.Dup || instr.OpCode.Code == CilCode.Pop; // lol
    }
}