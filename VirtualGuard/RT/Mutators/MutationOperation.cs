using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators;

public class MutationOperation
{
    public static VmCode ToOpCode(MutationType t)
    {
        switch (t)
        {
           case MutationType.Add:
               return VmCode.Add;
           
           case MutationType.Sub:
               return VmCode.Sub;
           
           case MutationType.Xor:
               return VmCode.Xor;
           
           default:
               throw new NotImplementedException(t.ToString());
        }
    }

    private static Random _rnd = new Random();
    
    public static VmInstruction[] MutateConstant(VmInstruction instr)
    {
        
        var mutations = _rnd.Next(4);
        var instrs = new List<VmInstruction>();

        var mutationOp = new MutationOperation(instr.Operand);
        
        // add initial
        instrs.Add(new VmInstruction(VmCode.Ldc_I4, mutationOp));

        for (int i = 0; i < mutations; i++)
        {


        }
        
        
        throw new NotImplementedException();
    }
    
    private object _iv;
    public MutationOperation(object obj)
    {
        _iv = obj;
    }

    int GetIV()
    {
        if (_iv is VmChunk chunk)
            return chunk.Offset;

        if (_iv is int i)
            return i;

        throw new InvalidOperationException(_iv.GetType().FullName);
    }
    
    public int Emulate()
    {
        Steps.Reverse(); // make the steps happen in the reverse order they execute in
        
        int initialValue = GetIV();
        
        foreach (var step in Steps)
        {
            initialValue = EmulateOp(initialValue, step);
        }

        return initialValue;
    }


    public List<MutationStep> Steps = new List<MutationStep>();

    MutationType InverseOp(MutationType type)
    {
        switch (type)
        {
            case MutationType.Add:
                return MutationType.Sub;
            
            case MutationType.Sub:
                return MutationType.Add;
            
            case MutationType.Xor:
                return MutationType.Xor;
            
            default:
                throw new NotImplementedException(type.ToString());
        }
        
    }

    int EmulateOp(int i1, MutationStep step)
    {
        var type = InverseOp(step.Type);
        switch (type)
        {
            case MutationType.Add:
                return i1 + step.Modifier;
            
            case MutationType.Sub:
                return i1 - step.Modifier;
            
            case MutationType.Xor:
                return i1 ^ step.Modifier;
            
            default:
                throw new NotImplementedException(type.ToString());
        }
    }
    
}

public struct MutationStep
{
    public int Modifier;
    public MutationType Type;
}

public enum MutationType
{
    Add,
    Sub,
    Xor
}
