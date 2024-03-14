namespace VirtualGuard.Runtime.OpCodes.impl.Stack
{

    public class Dup : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            ctx.Stack.Push(ctx.Stack.Peek().Clone());

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}