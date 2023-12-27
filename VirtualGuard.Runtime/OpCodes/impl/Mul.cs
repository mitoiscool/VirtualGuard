using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Mul : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var i2 = ctx.Stack.Pop().ToNumeral();
            var i1 = ctx.Stack.Pop().ToNumeral();

            ctx.Stack.Push(i1.Mul(i2));

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}