using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Not : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            ctx.Stack.Push(ctx.Stack.Pop().ToNumeral().Not());
            
            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}