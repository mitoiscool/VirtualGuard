using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST.IL;

namespace VirtualGuard.AST;

public class AstExpression
{
    public static AstExpression Create(CilInstruction instr, AstExpression[] args)
    {
        if (instr is Marker m)
            return new AstMarker(CilOpCodes.Nop, null, args)
            {
                Type = m.Type
            };

        // if not marker
        return new AstExpression(instr.OpCode, instr.Operand, args);
    }
    
    public AstExpression(CilOpCode opCode, object operand, AstExpression[] exprs)
    {
        OpCode = opCode;
        Operand = operand;
        Arguments = exprs;
    }

    public AstExpression[] Arguments;

    public CilOpCode OpCode;
    public object Operand;
    
}