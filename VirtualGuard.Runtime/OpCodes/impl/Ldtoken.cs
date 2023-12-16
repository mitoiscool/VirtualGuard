using System.Reflection;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Ldtoken : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        var member = ctx.ResolveMember(ctx.Reader.ReadInt().I4());
        
        state = ExecutionState.Next;

        if (member is TypeInfo t)
        {
            ctx.Stack.Push(new ObjectVariant(t.TypeHandle));
            return;
        }


        if (member is MethodInfo m)
        {
            ctx.Stack.Push(new ObjectVariant(m.MethodHandle));
            return;
        }

        if (member is FieldInfo f)
        {
            ctx.Stack.Push(new ObjectVariant(f.FieldHandle));
            return;
        }

        throw new InvalidOperationException(
            Routines.EncryptDebugMessage("ldtoken resolution not typeinfo methodinfo or fieldinfo"));
    }

    public byte GetCode() => 0;
}