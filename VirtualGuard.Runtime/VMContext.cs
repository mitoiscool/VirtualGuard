using System;
using System.Collections.Generic;
using System.Reflection;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.OpCodes;
using VirtualGuard.Runtime.Regions;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime
{

    public class VMContext
    {
        public VMContext(byte entryKey)
        {
            _entryKey = entryKey;
        }

        private byte _entryKey;
        public readonly VMStack Stack = new();
        public readonly LocalStorage Locals = new();
        public readonly VMReader Reader = new();

        public Exception Exception;

        public Stack<BaseRegion> CatchStack = new Stack<BaseRegion>();

        public object Dispatch(int loc, object[] args)
        {
            Reader.SetValue(loc);
            Reader.SetKey(_entryKey);
            
            Stack.Push(new ArrayVariant(args));

            do
            {
                try
                {
                    var handler = Reader.ReadHandler();
                    
#if DEBUG
                    Console.WriteLine(CodeMap.LookupCode(handler).GetType().Name);
#endif
                    
                    CodeMap.LookupCode(handler).Execute(this, out ExecutionState state);
                    
                    if (state != ExecutionState.Next)
                        break;

                }
                catch (Exception ex)
                {
                    if (CatchStack.Count == 0)
                        throw ex;
                    
                    Unwind();
                }

            } while (true);
            

            return Stack.Pop().GetObject();
        }


        void Unwind()
        {
            var handler = CatchStack.Pop();
            
            Stack.SetValue(0); // reset stack ptr
            
            // handler needs to jump to finally if we want finally to work
            
            Reader.SetKey(handler.EntryKey);
            Reader.SetValue(handler.Position);
            
            Stack.Push(new ObjectVariant(Exception));
        }
        
        
        public MethodBase ResolveMethod(int i)
        {
            return (MethodBase)Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

        public FieldInfo ResolveField(int i)
        {
            return (FieldInfo)Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

        public Type ResolveType(int i)
        {
            return Assembly.GetExecutingAssembly().ManifestModule.ResolveType(i);
        }

        public MemberInfo ResolveMember(int i)
        { 
            return Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

    }
}