using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ldc_I4 : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            ctx.Stack.Push(new IntVariant(ctx.Reader.ReadInt()));

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}