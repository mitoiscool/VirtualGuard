using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.RT.Descriptor;
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
            
            
            // inject mutations for fixups
            
            if(kvp.Key == VmCode.Jmp)
                continue;
            
            // find execute method
            var execute = kvp.Value.Methods.Single(x => x.Name == "Execute");
            
            if(execute.CilMethodBody == null || execute.CilMethodBody.Instructions.Count == 0)
                continue;

            var fixupRefs = execute.CilMethodBody.Instructions.Where(x =>
                x.Operand is IMethodDescriptor fd && fd.Name == "ReadFixupValue").ToArray();

            var mutationCil = rt.Descriptor.Data.GetFixupMutationCil(kvp.Key);

            foreach (var fixupRef in kvp.Key == VmCode.Jz ? fixupRefs.Skip(1) : fixupRefs)
            { // skip first ref for jz, second is the one that matters
                
                // get index of fixupRef
                var index = execute.CilMethodBody.Instructions.IndexOf(fixupRef) + 1;
                
                // insert all into target
                execute.CilMethodBody.Instructions.InsertRange(index, mutationCil);
            }
            
            execute.CilMethodBody.Instructions.CalculateOffsets();
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
                    
                    case "HEADER_IV":
                        inlinedConstant = rt.Descriptor.Data.InitialHeaderKey;
                        break;
                    
                    case "HEADER_ROTATION_FACTOR1":
                        inlinedConstant = rt.Descriptor.Data.HeaderRotationFactors[0];
                        break;
                    
                    case "HEADER_ROTATION_FACTOR2":
                        inlinedConstant = rt.Descriptor.Data.HeaderRotationFactors[1];
                        break;
                    
                    case "HEADER_ROTATION_FACTOR3":
                        inlinedConstant = rt.Descriptor.Data.HeaderRotationFactors[2];
                        break;
                    
                    case "CorlibID_I":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.I;
                        break;
                    
                    case "CorlibID_I1":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.I1;
                        break;
                    
                    case "CorlibID_I2":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.I2;
                        break;
                    
                    case "CorlibID_I4":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.I4;
                        break;
                    
                    case "CorlibID_I8":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.I8;
                        break;
                    
                    case "CorlibID_U":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.U;
                        break;
                    
                    case "CorlibID_U1":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.U1;
                        break;
                    
                    case "CorlibID_U2":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.U2;
                        break;
                    
                    case "CorlibID_U4":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.U4;
                        break;
                    
                    case "CorlibID_U8":
                        inlinedConstant = rt.Descriptor.CorLibTypeDescriptor.U8;
                        break;
                    
                    case "CatchFL":
                        inlinedConstant = rt.Descriptor.ExceptionHandlers.CatchFL;
                        break;
                    
                    case "FinallyFL":
                        inlinedConstant = rt.Descriptor.ExceptionHandlers.FinallyFL;
                        break;
                    
                    case "FilterFL":
                        inlinedConstant = rt.Descriptor.ExceptionHandlers.FilterFL;
                        break;
                    
                    case "FaultFL":
                        inlinedConstant = rt.Descriptor.ExceptionHandlers.FaultFL;
                        break;
                    
                    case "NKey":
                        inlinedConstant = rt.Descriptor.HashDescriptor.NKey;
                        break;
                    
                    case "NSalt1":
                        inlinedConstant = rt.Descriptor.HashDescriptor.NSalt1;
                        break;
                    
                    case "NSalt2":
                        inlinedConstant = rt.Descriptor.HashDescriptor.NSalt2;
                        break;
                    
                    case "NSalt3":
                        inlinedConstant = rt.Descriptor.HashDescriptor.NSalt3;
                        break;
                    
                    case "SPolynomial":
                        inlinedConstant = rt.Descriptor.HashDescriptor.SPolynomial;
                        break;
                    
                    case "SSeed":
                        inlinedConstant = rt.Descriptor.HashDescriptor.SSeed;
                        break;
                    
                    case "SXorMask":
                        inlinedConstant = rt.Descriptor.HashDescriptor.SXorMask;
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
                
                if(inlinedConstant is uint ui) // fml
                    cilInstruction.ReplaceWith(CilOpCodes.Ldc_I4, (int)ui);
            }
            
        }

        rt.RuntimeModule.TopLevelTypes.Remove(constantsType); // constants inlined, now can remove the type

    }
    
    
    
}