using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldflda : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var field = ctx.ResolveField(ctx.Stack.Pop().I4());

            object inst = null;

            if (!field.IsStatic)
                inst = ctx.Stack.Pop();
            
            ctx.Stack.Push(new FieldReferenceVariant(field, inst)); // no value here, interesting

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}