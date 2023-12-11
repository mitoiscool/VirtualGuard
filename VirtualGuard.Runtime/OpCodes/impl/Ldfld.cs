using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ldfld : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var field = ctx.ResolveField(ctx.Stack.Pop().I4());

            object inst = null;

            if (!field.IsStatic) // ldsfld
                inst = ctx.Stack.Pop();

            var value = BaseVariant.CreateVariant(field.GetValue(inst));

            ctx.Stack.Push(value);

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}