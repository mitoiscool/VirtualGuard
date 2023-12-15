using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

public class InjectConstants : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {

        var opcodes = typeof(VmCode).GetEnumNames().Where(x => x.Substring(0, 2) != "__").ToArray(); // eliminate transform instrs
        var opcodeMap = new Dictionary<VmCode, TypeDefinition>();
        
        
        foreach (var name in opcodes)
        {
            try
            {
                var type = rt.RuntimeModule.LookupType(RuntimeConfig.BaseHandler + "." + name);

                opcodeMap.Add((VmCode)Array.IndexOf(opcodes, name), type);
            }
            catch(InvalidOperationException ex)
            {
                throw new KeyNotFoundException("Could not locate opcode: " + name + " in runtime!");
            }
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
        
        // inject comparison flags

        var constantsType = rt.RuntimeModule.LookupType(RuntimeConfig.Constants);

        

        foreach (var method in rt.RuntimeModule.GetAllTypes().SelectMany(x => x.Methods).Where(x => x.CilMethodBody != null && x.CilMethodBody.Instructions.Count > 0))
        { // iterate through all instructions in vm type (is there a faster way to do this?)
            
            foreach (var cilInstruction in method.CilMethodBody.Instructions)
            {
                
                if(cilInstruction.OpCode.Code != CilCode.Ldsfld)
                    continue;
            
                if(cilInstruction.Operand == null)
                    continue;

                var field = (IFieldDescriptor)cilInstruction.Operand;

                if(field.DeclaringType != constantsType)
                    continue;
                
                // this is a bad way of doing this

                object inlinedConstant = 0; // this is nice bc if data is not put in field it defaults to 0

                switch (field.Name)
                {
                    case "CMP_GT":
                        inlinedConstant = rt.Descriptor.ComparisonFlags.GtFlag;
                        break;
                    
                    case "CMP_LT":
                        inlinedConstant = rt.Descriptor.ComparisonFlags.LtFlag;
                        break;
                    
                    case "CMP_EQ":
                        inlinedConstant = rt.Descriptor.ComparisonFlags.EqFlag;
                        break;
                    
                    case "DT_WATERMARK":
                        inlinedConstant = rt.Descriptor.Data.Watermark;
                        break;
                    
                    case "DT_NAME":
                        inlinedConstant = rt.Descriptor.Data.StreamName;
                        break;
                    
                    case "HANDLER_ROT1":
                        inlinedConstant = rt.Descriptor.Data.HandlerShifts[0];
                        break;
                    
                    case "HANDLER_ROT2":
                        inlinedConstant = rt.Descriptor.Data.HandlerShifts[1];
                        break;
                    
                    case "HANDLER_ROT3":
                        inlinedConstant = rt.Descriptor.Data.HandlerShifts[2];
                        break;
                    
                    case "HANDLER_ROT4":
                        inlinedConstant = rt.Descriptor.Data.HandlerShifts[3];
                        break;
                    
                    case "HANDLER_ROT5":
                        inlinedConstant = rt.Descriptor.Data.HandlerShifts[4];
                        break;
                    
                    case "BYTE_ROT1":
                        inlinedConstant = rt.Descriptor.Data.ByteShifts[0];
                        break;
                    case "BYTE_ROT2":
                        inlinedConstant = rt.Descriptor.Data.ByteShifts[1];
                        break;
                    case "BYTE_ROT3":
                        inlinedConstant = rt.Descriptor.Data.ByteShifts[2];
                        break;
                    case "BYTE_ROT4":
                        inlinedConstant = rt.Descriptor.Data.ByteShifts[3];
                        break;
                    case "BYTE_ROT5":
                        inlinedConstant = rt.Descriptor.Data.ByteShifts[4];
                        break;
                    
                    default:
                        inlinedConstant = 0; // default value to inline
                        
                        // or maybe a better idea to throw

                        throw new NotImplementedException("No inline constant specified for constant " + field.Name);
                }
                
                
                // now inline the value
                
                if(inlinedConstant is string s)
                    cilInstruction.ReplaceWith(CilOpCodes.Ldstr, s);
                
                if(inlinedConstant is byte b)
                    cilInstruction.ReplaceWith(CilOpCodes.Ldc_I4, (int)b);
                
                if(inlinedConstant is int i)
                    cilInstruction.ReplaceWith(CilOpCodes.Ldc_I4, i);
            }
            
        }

        rt.RuntimeModule.TopLevelTypes.Remove(constantsType); // constants inlined, now can remove the type

    }
    
    
    
}