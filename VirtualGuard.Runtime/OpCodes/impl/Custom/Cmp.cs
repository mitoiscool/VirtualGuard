using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl.Misc
{

    public class Cmp : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var v2 = ctx.Stack.Pop();
            var v1 = ctx.Stack.Pop();
            
            // could do something cool where we have a custom conditional branch based off of a flag
            // flag type would be onstack followed by the actual flag
            
            ctx.Stack.Push(new IntVariant(BaseVariant.Compare(v1, v2)));

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}