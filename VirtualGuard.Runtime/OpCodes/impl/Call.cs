using System;
using System.Reflection;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.Reference;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Call : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var methodBase = ctx.ResolveMethod(ctx.Reader.ReadInt().I4());


            // pop args

            var args = methodBase.GetParameters();
            var argsCasted = new object[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                // this doesn't support ref args
                argsCasted[i] = Convert.ChangeType(ctx.Stack.Pop().GetObject(), args[i].ParameterType);
            }

            // this is such a shitty way of doing this
            
            object inst;

            if (!methodBase.IsStatic)
            {
                BaseVariant instVar = null;
                instVar = ctx.Stack.Pop();

                if (instVar.IsReference())
                {
                    inst = ((BaseReferenceVariant)instVar).GetPtr();
                }
                else
                {
                    inst = instVar.GetObject();
                }
            }
            else
            {
                inst = null;
            }

            var ret = methodBase.Invoke(inst, argsCasted);

            // terms for ret

            // if it is a ctor, inst should be returned always
            if (methodBase.Name == ".ctor" || methodBase is MethodInfo mi && mi.ReturnType != typeof(void))
            {
                // should return
                ctx.Stack.Push(new ObjectVariant(ret));
            }

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}