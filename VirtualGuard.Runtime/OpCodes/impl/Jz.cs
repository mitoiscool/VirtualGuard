using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Jz : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var condLoc = ctx.Stack.Pop();

            var flag = ctx.Stack.Pop().I2();
            var key = ctx.Reader.ReadByte();
            
            if (flag == 0)
            {
                ctx.Reader.SetKey(key.U1());
                ctx.Reader.SetValue(condLoc.I4());
            }

            ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}