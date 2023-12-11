using System.Collections.Generic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Vmcall : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var argCount = ctx.Reader.ReadInt().I4();
            var loc = ctx.Stack.Pop().I4();
            
            var args = new List<object>();
            
            for(int i = 0; i < argCount; i++)
                args.Add(ctx.Stack.Pop());

            ctx.Stack.Push(BaseVariant.CastVariant(Entry.VMEntry(loc, args.ToArray())));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}