using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.RT.Mutators.impl.Runtime;

internal class LocMutation : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        var rnd = new Random();
        
        foreach (var method in rt.RuntimeModule.GetAllTypes().SelectMany(x => x.Methods.Where(x => x.CilMethodBody != null && x.CilMethodBody.Instructions.Count > 0)))
        {
            method.CilMethodBody.Instructions.ExpandMacros();
            
            foreach (var instr in method.CilMethodBody.Instructions.ToArray())
            {
                //if (rnd.Next(2) != 0) // 1/3
                //    continue;

                if (instr.OpCode == CilOpCodes.Stloc)
                {
                    var loc = instr.Operand;

                    method.CilMethodBody.Instructions.Insert(method.CilMethodBody.Instructions.IndexOf(instr), CilOpCodes.Ldnull);
                    
                    method.CilMethodBody.Instructions.InsertRange(method.CilMethodBody.Instructions.IndexOf(instr), new[]
                    {
                        new CilInstruction(CilOpCodes.Stloc,
                            loc),
                    });
                }
            }
            
            method.CilMethodBody.Instructions.OptimizeMacros();
            method.CilMethodBody.ComputeMaxStack();
        }
    }
}