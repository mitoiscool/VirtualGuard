using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl.Constant
{

    public class Ldc_I8 : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            ctx.Stack.Push(new LongVariant(ctx.Reader.ReadLong()));

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}