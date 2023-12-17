using System.Text;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.RT.Mutators.impl;

public class EncryptExceptions : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if(rt.isDebug)
            return;
        // locate routine type
        var routineType = rt.RuntimeModule.LookupType(RuntimeConfig.Routines);

        var encryptDebugSymbol = routineType.Methods.Single(x => x.Name == "EncryptDebugMessage");

        foreach (var method in rt.RuntimeModule.TopLevelTypes.SelectMany(x => x.Methods)
                     .Where(x => x.CilMethodBody != null && x.CilMethodBody.Instructions.Count > 0))
        {

            bool foundInstance = false;

            foreach (var instr in method.CilMethodBody.Instructions.Reverse())
            {
                if (foundInstance)
                {
                    if(instr.OpCode.Code != CilCode.Ldstr)
                        continue;

                    var str = instr.Operand.ToString();

                    instr.Operand = Encrypt(str, rt.Descriptor.Data.DebugKey);
                    
                    foundInstance = false; // set to false again
                }
                
                if(instr.OpCode.Code != CilCode.Call)
                    continue;

                if (instr.Operand is IMethodDescriptor mdesc && mdesc.FullName == encryptDebugSymbol.FullName)
                {
                    instr.ReplaceWithNop();
                    foundInstance = true;
                }
                
                
            }

            routineType.Methods.Remove(encryptDebugSymbol);

        }
        
    }

    string Encrypt(string str, int key)
    {
        StringBuilder builder = new StringBuilder();
        foreach (char c in str.ToCharArray())
            builder.Append((char)(c - key));

        return builder.ToString();
    }
    
}