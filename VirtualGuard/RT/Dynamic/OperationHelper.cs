using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.RT.Dynamic;

public static class OperationHelper
{
    private static readonly Dictionary<Operation, Operation> InverseOperations = new Dictionary<Operation, Operation>()
    {
        { Operation.Add, Operation.Sub},
        { Operation.Sub, Operation.Add },
        { Operation.Xor, Operation.Xor },
    };

    private static readonly Dictionary<Operation, CilOpCode> OperationCodeMap = new Dictionary<Operation, CilOpCode>()
    {
        { Operation.Add, CilOpCodes.Add },
        { Operation.Sub, CilOpCodes.Sub },
        { Operation.Xor, CilOpCodes.Xor },
    };

    public static Operation Inverse(this Operation op)
    {
        return InverseOperations[op];
    }

    public static CilOpCode ToCil(this Operation op)
    {
        return OperationCodeMap[op];
    }

    public static int Operate(this Operation op, int operand1, int operand2)
    {
        switch (op)
        {
            case Operation.Add:
                return operand1 + operand2;
            
            case Operation.Sub:
                return operand1 - operand2;

            case Operation.Xor:
                return operand1 ^ operand2;


            default:
                throw new NotImplementedException(op.ToString());
        }
    }
    
} 