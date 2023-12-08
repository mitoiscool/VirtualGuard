using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

public class InjectConstants : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt)
    {

        var opcodes = typeof(VmCode).GetEnumNames().Where(x => x.Substring(0, 2) != "__").ToArray(); // eliminate transform instrs
        var opcodeMap = new Dictionary<VmCode, TypeDefinition>();

        foreach (var name in opcodes)
        {
            var type = rt.RuntimeModule.LookupType(RuntimeConfig.BaseHandler + "." + name);
            
            Console.WriteLine("t: {0} o: {1}", type.FullName, name);
            opcodeMap.Add((VmCode)Array.IndexOf(opcodes, name), type);
        }


        foreach (var kvp in opcodeMap)
        {
            // wait I'm completely bypassing the idea of the constants class here lol
            
            // find method in type
            var getCode = kvp.Value.Methods.Single(x => x.Name == "GetCode");

            var instrs = getCode.CilMethodBody.Instructions;
            
            instrs.Clear();

            instrs.Add(CilOpCodes.Ldc_I4, rt.Descriptor.OpCodes[kvp.Key]);
            instrs.Add(CilOpCodes.Ret);
        }
        
        
        
    }
    
    
    
}