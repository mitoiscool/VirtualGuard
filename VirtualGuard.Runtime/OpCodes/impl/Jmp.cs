using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Jmp : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var key = ctx.Reader.ReadByte();
            
            ctx.Reader.SetKey(key.U1());
            ctx.Reader.SetValue(ctx.Stack.Pop().I4());
            
            ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}