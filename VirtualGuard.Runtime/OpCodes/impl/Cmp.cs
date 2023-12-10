using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Flags;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Cmp : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var v1 = ctx.Stack.Pop();
            var v2 = ctx.Stack.Pop();
            
            if (BaseVariant.Compare(v1, v2) == (byte)ComparisonFlags.EQ)
            {
                ctx.Stack.Push(new ShortVariant(1));
            }
            else
            {
                ctx.Stack.Push(new ShortVariant(0));
            }

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}