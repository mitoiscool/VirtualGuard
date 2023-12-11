using System;
using System.Collections.Generic;
using System.Reflection;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.OpCodes;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime
{

    public class VMContext
    {
        public VMContext()
        {
            Stack = new VMStack();
            Reader = new VMReader();
            Locals = new LocalStorage();
        }

        public VMStack Stack;
        public LocalStorage Locals;
        public VMReader Reader;
        private Dictionary<short, BaseVariant> _locals = new Dictionary<short, BaseVariant>();

        private Exception _exception;

        public object Dispatch(int loc, object[] args)
        {
            Reader.SetValue(loc);
            Stack.Push(new ArrayVariant(args));

            switch (DispatchInternal())
            {
                case ExecutionState.Catch:
                    // handle _exception, check for finally and potentially jump to that state in the case

                    // if finally, goto case finally
                    break;

                case ExecutionState.Finally:
                    // jump to handler location 
                    break;
            }

            return Stack.Pop();
        }

        ExecutionState DispatchInternal()
        {
            ExecutionState state;

            do
            {
                try
                {
                    var handler = Reader.ReadHandler();

                    CodeMap.GetCode(handler).Execute(this, out state);

                    if (state != ExecutionState.Next)
                        break;

                }
                catch (Exception ex)
                {
                    _exception = ex;
                    return ExecutionState.Catch;
                }

            } while (true);

            return state;
        }

        public MethodBase ResolveMethod(int i)
        {
            return (MethodBase)Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

        public FieldInfo ResolveField(int i)
        {
            return (FieldInfo)Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

    }
}