using System.Collections.Generic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Vmcall : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var loc = ctx.Stack.Pop().I4();
            var argCount = ctx.Reader.ReadInt().I4();
            
            var entryKey = ctx.Reader.ReadByte();
            
            var args = new List<object>();
            
            // need to add support for ref params
            
            for(int i = 0; i < argCount; i++)
                args.Add(ctx.Stack.Pop().GetObject());

            // do i need to cast here?
            // it may be better to restructure so vm doesn't need to init args
            
            ctx.Stack.Push(BaseVariant.CreateVariant(Entry.VMEntry(loc, entryKey.U1(), args.ToArray())));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}