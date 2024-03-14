namespace VirtualGuard.Runtime.OpCodes.impl.Array;

public class Ldlen : IOpCode
{
    public void Execute(VMContext ctx)
    {
        ctx.Stack.Push(ctx.Stack.Pop().ToArray().GetLength());
        
        ctx.CurrentCode += ctx.Reader.ReadFixupValue();
        CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
    }

    public byte GetCode() => 0;
}