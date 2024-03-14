namespace VirtualGuard.Runtime.OpCodes.impl.Bitwise
{
    public class Or : IOpCode
    {
        public void Execute(VMContext ctx)
        { 
            var i2 = ctx.Stack.Pop().ToNumeral();
            var i1 = ctx.Stack.Pop().ToNumeral();

            ctx.Stack.Push(i1.Or(i2));

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}