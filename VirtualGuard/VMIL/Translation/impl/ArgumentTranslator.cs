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

        if (instr.OpCode.Code == CilCode.Ldarg)
        {
            block.WithContent(
                new VmInstruction(VmCode.Ldloc, meth.GetVariableFromArg(index)));
        }
        else
        {
            block.WithContent(
                new VmInstruction(VmCode.Ldloca, meth.GetVariableFromArg(index)));
        }
        
    }

    [Obfuscation(Feature = "virtualization")]
    public bool Supports(AstExpression instr)
    {
        if (instr.OpCode == CilOpCodes.Ldarg)
            return true;

        if (instr.OpCode == CilOpCodes.Ldarga)
            return true;

        return false;
    }
}