using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldloca : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var local = ctx.Locals.GetLocal(ctx.Reader.ReadShort());
            
            ctx.Stack.Push(new LocalReferenceVariant(local));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
        
    }
}