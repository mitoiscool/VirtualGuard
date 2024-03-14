using System.Reflection;
using AsmResolver.DotNet.Collections;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

internal class ArgumentTranslator : ITranslator
{
    [Obfuscation(Feature = "virtualization")]
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx)
    {
        var param = instr.Operand as Parameter;
        var index = param.Index;
        
        var varFromArg = meth.GetVariableFromArg(index);

        switch (instr.OpCode.Code)
        {
            case CilCode.Ldarg:
                block.WithContent(
                    new VmInstruction(VmCode.Ldloc, varFromArg));
                break;
            
            case CilCode.Ldarga:
                block.WithContent(
                    new VmInstruction(VmCode.Ldloca, varFromArg));
                break;
            
            case CilCode.Starg:
                block.WithContent(new VmInstruction(VmCode.Stloc, varFromArg));
                break;
        }
        
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        if (instr.OpCode == CilOpCodes.Ldarg)
            return true;

        if (instr.OpCode == CilOpCodes.Ldarga)
            return true;

        if (instr.OpCode == CilOpCodes.Starg)
            return true;

        return false;
    }
}