namespace VirtualGuard.Runtime.Execution;

public class ExceptionHandler
{
    public ExceptionHandler(byte t, byte start, int handlerPos)
    {
        Type = t;
        HandlerStartKey = start;
        HandlerPos = handlerPos;
    }
    
    public byte Type;
    
    public byte HandlerStartKey;
    public int HandlerPos;
}