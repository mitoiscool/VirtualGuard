using AsmResolver.DotNet.Collections;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ArgumentTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth)
    {
        var param = instr.Operand as Parameter;

        if (instr.OpCode.Code == CilCode.Ldarg)
        {
            block.WithContent(
                new VmInstruction(VmCode.Ldloc, meth.GetVariableFromArg(param.Index)));
        }
        else
        {
            block.WithContent(
                new VmInstruction(VmCode.Ldloca, meth.GetVariableFromArg(param.Index)));
        }
        
    }

    public bool Supports(CilInstruction instr)
    {
        if (instr.OpCode == CilOpCodes.Ldarg)
            return true;

        if (instr.OpCode == CilOpCodes.Ldarga)
            return true;

        return false;
    }
}