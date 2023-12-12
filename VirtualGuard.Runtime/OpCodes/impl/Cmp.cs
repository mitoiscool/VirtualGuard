using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Cmp : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var v2 = ctx.Stack.Pop();
            var v1 = ctx.Stack.Pop();
            
            // could do something cool where we have a custom conditional branch based off of a flag
            // flag type would be onstack followed by the actual flag
            
            ctx.Stack.Push(new IntVariant(BaseVariant.Compare(v1, v2)));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}