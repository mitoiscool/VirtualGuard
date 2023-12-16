using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class CallTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {
        //if(method.Resolve())
        
        block.WithContent(new VmInstruction(VmCode.Call, instr.Operand));
    }

    public bool Supports(CilInstruction instr)
    {
        return new[]
        {
            CilCode.Newobj,
            CilCode.Call,
            CilCode.Callvirt
        }.Contains(instr.OpCode.Code);
        
    }
}