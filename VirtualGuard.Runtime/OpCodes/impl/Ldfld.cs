using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ldfld : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {

            var field = ctx.Stack.Pop().ToField();

            var value = BaseVariant.CastVariant(field.GetValue(ctx.Stack.Pop()));

            ctx.Stack.Push(value);

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}