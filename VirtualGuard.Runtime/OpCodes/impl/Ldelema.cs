using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldelema : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        { // need to make arrayreferencevariant

            var arr = ctx.Stack.Pop().ToArray();
            var index = ctx.Stack.Pop();

            ctx.Stack.Push(new ArrayReferenceVariant(arr, index));
            
            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}