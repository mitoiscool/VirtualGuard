namespace VirtualGuard.Runtime.OpCodes.impl.Bitwise;

public class Shl : IOpCode
{
    public void Execute(VMContext ctx)
    {
        var factor = ctx.Stack.Pop();
        var num = ctx.Stack.Pop();
        
        ctx.Stack.Push(num.ToNumeral().Shl(factor.ToNumeral()));
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}