using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Echo.Platforms.AsmResolver;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class LocalTranslator : ITranslator
{
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth)
    {
        var local = instr.Operand as CilLocalVariable;
        var vmLoc = meth.GetVariableFromLocal(local.Index);
        
        switch (instr.OpCode.Code)
        {
            case CilCode.Stloc:
                block.WithContent(
                    new VmInstruction(VmCode.Stloc, vmLoc));
                break;
            
            case CilCode.Ldloca:
                block.WithContent(
                    new VmInstruction(VmCode.Ldloca, vmLoc));
                break;
            
            case CilCode.Ldloc:
                block.WithContent(
                    new VmInstruction(VmCode.Ldloc, vmLoc));
                break;
        }
    }

    public bool Supports(CilInstruction instr)
    {
        if (instr.OpCode.Code == CilCode.Stloc)
            return true;
        
        if (instr.OpCode.Code == CilCode.Ldloc)
            return true;
        
        if (instr.OpCode.Code == CilCode.Ldloca)
            return true;
        
        return false;
    }
}