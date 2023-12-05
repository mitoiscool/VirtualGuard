using AsmResolver.DotNet.Collections;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ArgumentTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth)
    {
        var param = instr.Operand as Parameter;
        
        block.WithContent(
            new VmInstruction(VmCode.Ldloc, meth.GetVariableFromArg(param.Index)));
    }

    public bool Supports(CilInstruction instr)
    {
        if (instr.OpCode == CilOpCodes.Ldarg)
            return true;

        return false;
    }
}