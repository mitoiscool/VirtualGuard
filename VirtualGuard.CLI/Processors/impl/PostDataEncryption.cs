using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.CLI.Processors.impl;

public class PostDataEncryption : IProcessor
{
    public string Identifier => "PostDataEncryption";
    public void Process(Context ctx)
    {

        foreach (var method in DataEncryption.ScannedMethods)
        {
            if(method.CilMethodBody == null)
                continue;
            
            method.CilMethodBody.Instructions.ExpandMacros();

            foreach (var instruction in method.CilMethodBody.Instructions.ToArray())
            {
                
                if(instruction.OpCode != CilOpCodes.Ldstr && instruction.OpCode != CilOpCodes.Ldc_I4)
                    continue;
                
                // replace w vm call
                
                method.CilMethodBody.Instructions.ReplaceRange(instruction, DataEncryption._constantCache[instruction.Operand].CilMethodBody.Instructions.SkipLast(1).ToArray());
            }
            
            method.CilMethodBody.Instructions.CalculateOffsets();
            method.CilMethodBody.Instructions.OptimizeMacros();
        }
        
        
        foreach (var meth in DataEncryption._constantCache.Values)
        { // cleanup
            meth.DeclaringType.Methods.Remove(meth);
        }
        
    }
}