using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.AST.IL;

namespace VirtualGuard.AST;


public class AstMarker : AstExpression
{
    public AstMarker(CilOpCode opCode, object operand, AstExpression[] exprs) : base(opCode, operand, exprs)
    {
    }

    public MarkerType Type;
}