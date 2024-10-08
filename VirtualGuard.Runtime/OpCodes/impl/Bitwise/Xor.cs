namespace VirtualGuard.Runtime.OpCodes.impl.Bitwise
{
    public class Xor : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var i2 = ctx.Stack.Pop().ToNumeral();
            var i1 = ctx.Stack.Pop().ToNumeral();

            ctx.Stack.Push(i1.Xor(i2));
            
            // basic jmp routine
            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}