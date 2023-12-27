using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Rem : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var i2 = ctx.Stack.Pop().ToNumeral();
            var i1 = ctx.Stack.Pop().ToNumeral();

            ctx.Stack.Push(i1.Rem(i2));
            
            ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}