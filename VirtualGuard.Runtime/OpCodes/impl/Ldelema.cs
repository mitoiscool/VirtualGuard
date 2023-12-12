using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldelema : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        { // need to make arrayreferencevariant

            var delem = ctx.Stack.Pop();
            var arr = ctx.Stack.Pop().ToArray();

            ctx.Stack.Push(new ArrayReferenceVariant(arr, delem));
            
            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}