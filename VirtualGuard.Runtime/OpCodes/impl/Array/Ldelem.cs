namespace VirtualGuard.Runtime.OpCodes.impl.Array
{
    public class Ldelem : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var delem = ctx.Stack.Pop();
            var arr = ctx.Stack.Pop().ToArray();
            
            ctx.Stack.Push(arr.LoadDelimeter(delem));

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}