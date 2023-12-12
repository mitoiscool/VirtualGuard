using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
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
            
            var args = methodBase.GetParameters();
            var vmVariantArgs = new BaseVariant[args.Length];
            var argsCasted = new object[args.Length];

            var refIndexes = new HashSet<int>();

            for (int i = 0; i < args.Length; i++)
            {
                vmVariantArgs[i] = ctx.Stack.Pop();

                if (vmVariantArgs[i].IsReference())
                    refIndexes.Add(i); // mark as reference to set value to after

                if (vmVariantArgs[i].GetObject().GetType() is IConvertible conv)
                {
                    argsCasted[i] = Convert.ChangeType(conv, args[i].ParameterType);
                }
                else
                {
                    argsCasted[i] = vmVariantArgs[i].GetObject();
                }
            }

            object inst = null;

            if (!methodBase.IsStatic && argsCasted.Length > 0)
            {
                // if inst, use first arg as inst
                inst = argsCasted[0];

                // this should preserve the refs
                argsCasted = argsCasted.Skip(1).ToArray();
            }

            object ret = methodBase.Invoke(inst, argsCasted);
            
            // update ref vars
            foreach (var refIndex in refIndexes) 
                vmVariantArgs[refIndex].SetValue(argsCasted[refIndex]); // set value of associated reference variant to that of the new variable
            
            
            if(methodBase.IsConstructor || methodBase is MethodInfo mi && mi.ReturnType != typeof(void))
                ctx.Stack.Push(BaseVariant.CreateVariant(ret));

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}