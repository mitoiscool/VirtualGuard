using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.OpCodes;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime;

public class VMContext
{
    public VMContext(VMData data)
    {
        Stack = new VMStack();
        Reader = new VMReader(data);
    }
    
    public VMStack Stack;
    public VMReader Reader;

    private Exception _exception;

    public object Dispatch(object[] args)
    {
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
    
}