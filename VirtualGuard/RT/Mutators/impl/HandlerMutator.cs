using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace VirtualGuard.RT.Mutators.impl;

internal class HandlerMutator : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        var rnd = new Random();

        foreach (var handler in rt.RuntimeModule.TopLevelTypes.Where(x => x.Interfaces.Any(x => x.Interface != null && x.Interface.Name == "IOpCode")))
        {
            // find random instruction to insert on
            var executeMethod = handler.Methods.Single(x => x.Name == "Execute");

            var instrs = executeMethod.CilMethodBody.Instructions;

            // random l2f

            Dictionary<CilLocalVariable, FieldDefinition> map = new Dictionary<CilLocalVariable, FieldDefinition>();

            foreach (var localVar in executeMethod.CilMethodBody.LocalVariables)
            {
                if(rnd.Next(2) != 0)
                    continue;

                var field = new FieldDefinition("vg", FieldAttributes.Private, localVar.VariableType);
                
                map.Add(localVar, field);
                handler.Fields.Add(field);
            }

            instrs.ExpandMacros();
            foreach (var instr in instrs.ToArray())
            {
                if(instr.OpCode == CilOpCodes.Ldloc && map.TryGetValue((CilLocalVariable)instr.Operand, out FieldDefinition fd))
                    instrs.ReplaceRange(instr, new CilInstruction(CilOpCodes.Ldarg_0), new CilInstruction(CilOpCodes.Ldfld, fd));

                if(instr.OpCode == CilOpCodes.Ldloca && map.TryGetValue((CilLocalVariable)instr.Operand, out FieldDefinition fd2))
                    instrs.ReplaceRange(instr, new CilInstruction(CilOpCodes.Ldarg_0), new CilInstruction(CilOpCodes.Ldflda, fd2));

                if (instr.OpCode == CilOpCodes.Stloc &&
                    map.TryGetValue((CilLocalVariable)instr.Operand, out FieldDefinition fd1))
                { // slightly more complicated bc we need to set to the local
                    
                    instrs.ReplaceRange(instr, new CilInstruction(CilOpCodes.Stloc, instr.Operand), new CilInstruction(CilOpCodes.Ldarg_0), new CilInstruction(CilOpCodes.Ldloc, instr.Operand), new CilInstruction(CilOpCodes.Stfld, fd1));
                }
                    
            }
            
            instrs.OptimizeMacros();

            
        }
        
        
        
        
    }

    private List<CilInstruction[]> _cachedMutators;
    List<CilInstruction[]> GetMutators(ModuleDefinition rt)
    {
        if (_cachedMutators != null)
            return _cachedMutators;
        
        _cachedMutators = new List<CilInstruction[]>();
        
        // locate junk type
        var junkType = rt.LookupType(RuntimeConfig.JunkCode);

        foreach (var method in junkType.Methods)
        {
            if(method.Name[0] != '_')
                continue;
            
            if(method.Signature.ReturnsValue)
                method.CilMethodBody.Instructions.Last().ReplaceWith(CilOpCodes.Pop);
            
            _cachedMutators.Add(method.CilMethodBody.Instructions.ToArray()); // NOTE: WILL NOT PRESERVE EXCEPTION HANDLERS OR LOCALS
        }

        return _cachedMutators;
    }
}