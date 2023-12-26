using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Stelem : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var arr = ctx.Stack.Pop().ToArray();

            arr.SetDelimeter(ctx.Stack.Pop(), ctx.Stack.Pop());
            
            ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}