using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.RT.Mutators.impl;

public class RemoveDebug : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if(rt.isDebug)
            return;
        
        // locate routine type
        var routineType = rt.RuntimeModule.LookupType(RuntimeConfig.Routines);

        var printDebugSymbol = routineType.Methods.Single(x => x.Name == "PrintDebug");

        foreach (var method in rt.RuntimeModule.TopLevelTypes.SelectMany(x => x.Methods)
                     .Where(x => x.CilMethodBody != null && x.CilMethodBody.Instructions.Count > 0))
        {

            bool foundInstance = false;

            foreach (var instr in method.CilMethodBody.Instructions.Reverse())
            {
                if (foundInstance)
                {
                    if (instr.OpCode.Code != CilCode.Ldstr)
                        continue;

                    instr.ReplaceWithNop();

                    foundInstance = false; // set to false again
                }

                if (instr.OpCode.Code != CilCode.Call)
                    continue;

                if (instr.Operand is IMethodDescriptor mdesc && mdesc.FullName == printDebugSymbol.FullName)
                {
                    instr.ReplaceWithNop();
                    foundInstance = true;
                }


            }
        }
    }
}