using System.Diagnostics;
using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation.impl;

public class ComparisonTranslator : ITranslator
{
    public void Translate(AstExpression instr, VmBlock block, VmMethod meth, VirtualGuardContext ctx)
    {

        // handle hashing (this is gross code)

        if (instr.Arguments[0].OpCode == CilOpCodes.Ldc_I4 && instr.Arguments[0].Operand is int i)
        {
            // cmp( const, v1)
            // we need to hash this const as an int and set operand,
            // then we will use hash opcode to hash onstack code

            var hashedInt = Util.HashNumber(i, ctx.Runtime.Descriptor.HashDescriptor);

            var translatedInstrs = meth.GetTranslatedInstructions(instr.Arguments[0]); // ah we need to get the converted instruction
            
            Debug.Assert(translatedInstrs.Length == 1); // should always be 1 in this case, just translating const

            var translatedLdc = translatedInstrs.Last();
            
            // relentless checks so I don't actually kms debugging
            Debug.Assert(translatedLdc.OpCode == VmCode.Ldc_I4); // need to add support for I8, R4, R8, S

            translatedLdc.Operand = hashedInt;
            
            // we can do this sketch maneuver to switch the stack vars instead of properly doing it using ast traversal
            
            // pop already const arg off stack

            var holder = meth.GetTempVar();
            
            block.WithContent(
                new VmInstruction(VmCode.Stloc, holder),
                new VmInstruction(VmCode.Hash),
                new VmInstruction(VmCode.Ldloc, holder)
                );

        }
        
        if (instr.Arguments[1].OpCode == CilOpCodes.Ldc_I4 && instr.Arguments[1].Operand is int i2)
        {
            // cmp( v1, const)
            // we need to hash this const as an int and set operand,
            // then we will use hash opcode to hash onstack code

            var hashedInt = Util.HashNumber(i2, ctx.Runtime.Descriptor.HashDescriptor);

            instr.Arguments[1].Operand = hashedInt;
            
            // we can do this sketch maneuver to switch the stack vars instead of properly doing it using ast traversal
            
            // pop already const arg off stack
            
            block.WithContent(
                new VmInstruction(VmCode.Hash) // hash other variable, hashed constant would be 2nd on stack
            );

        }
        
        
        block.WithContent(new VmInstruction(VmCode.Cmp)); // main cmp
        
        // now we just need to push the branch condition, aka validating the output flag
        
        switch (instr.OpCode.Code)
        {
            case CilCode.Ceq:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, (int)meth.Runtime.Descriptor.ComparisonFlags.EqFlag));
                break;
            
            case CilCode.Cgt:
            case CilCode.Cgt_Un:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, (int)meth.Runtime.Descriptor.ComparisonFlags.GtFlag));
                break;
            
            case CilCode.Clt:
            case CilCode.Clt_Un:
                block.WithContent(new VmInstruction(VmCode.Ldc_I4, (int)meth.Runtime.Descriptor.ComparisonFlags.LtFlag));
                break;
        }
        
        // now we need to determine if the values are the same to produce the 1 or 0 flag
        
        block.WithContent(new VmInstruction(VmCode.Xor)); // this will push 0 if they are equal
    }

    public bool Supports(AstExpression instr)
    {
        return new[]
        {
            CilCode.Ceq,
            CilCode.Cgt,
            CilCode.Cgt_Un,
            CilCode.Clt,
            CilCode.Clt_Un,
        }.Contains(instr.OpCode.Code);
    }
    
}