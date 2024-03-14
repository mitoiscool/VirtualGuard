namespace VirtualGuard.Runtime.OpCodes.impl.Array
{
    public class Stelem : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var arr = ctx.Stack.Pop().ToArray();

            arr.SetDelimeter(ctx.Stack.Pop(), ctx.Stack.Pop());
            
            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}