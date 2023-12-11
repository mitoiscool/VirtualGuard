using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Stfld : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var field = ctx.ResolveField(ctx.Stack.Pop().I4());

            object inst = null;

            if (!field.IsStatic)
                inst = ctx.Stack.Pop();

            field.SetValue(inst, ctx.Stack.Pop());

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}