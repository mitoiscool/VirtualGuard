using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Not : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            ctx.Stack.Push(ctx.Stack.Pop().ToNumeral().Not());
            
            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}