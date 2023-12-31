using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

internal class BranchMutator : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if(rt.isDebug)
            return;
        
        
        var rnd = new Random();
        
        foreach (var chunk in rt.VmChunks)
        foreach (var instr in chunk.Content.ToArray())
        {
            if(instr.OpCode != VmCode.Ldc_I4)
                continue;
            
            if(instr.Operand is not VmChunk target)
                continue;
            
            
            // going to be silly and use cmp flags to mutate the target values

            var salt = rnd.Next(10000);
            // first alloc all instructions

            var cmpValue1 = new VmInstruction(VmCode.Ldc_I4, 0);
            var cmpValue2 = new VmInstruction(VmCode.Ldc_I4, 0);
            
            var cmpInstr = new VmInstruction(VmCode.Cmp);

            var missingValueInstr = new VmInstruction(VmCode.Ldc_I4, 0);
            var addInstr = new VmInstruction(VmCode.Xor);

            chunk.Content.ReplaceRange(instr,
                cmpValue1,
                cmpValue2,

                cmpInstr,

                missingValueInstr,
                addInstr,
                new VmInstruction(VmCode.Ldc_I4, salt),
                new VmInstruction(VmCode.Sub)
                );
            
            
            // we'll use eq flag to test

            byte flagValue = 0;
            
            int i1 = 0;
            int i2 = 0;

            switch (rnd.Next(2))
            {
                case 0: // gt
                    flagValue = rt.Descriptor.ComparisonFlags.GtFlag;
                    i2 = rnd.Next();
                    i1 = rnd.Next(i2, int.MaxValue);
                    break;
                
                case 1: // lt
                    flagValue = rt.Descriptor.ComparisonFlags.LtFlag;
                    i1 = rnd.Next();
                    i2 = rnd.Next(i1, int.MaxValue);
                    break;
                
                case 2: // eq
                    flagValue = rt.Descriptor.ComparisonFlags.LtFlag;
                    i1 = rnd.Next();
                    i2 = i1;
                    break;
            }
            
            // emulate eq by setting two of the same value
            cmpValue1.Operand = i1;
            cmpValue2.Operand = i2;

            var mutationExpression = new MutationOperation(target);
            
            mutationExpression.Steps.Add(new MutationStep() { Modifier = flagValue, Type = MutationType.Xor}); // xor
            mutationExpression.Steps.Add(new MutationStep() { Modifier = salt, Type = MutationType.Sub}); // salt

            missingValueInstr.Operand = mutationExpression; // pray this works edit: it works lfg
        }
        

    }
    
    
    
}