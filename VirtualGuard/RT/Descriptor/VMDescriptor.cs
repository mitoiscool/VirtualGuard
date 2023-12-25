namespace VirtualGuard.RT.Descriptor;

public class VMDescriptor
{
    public OpCodeDescriptor OpCodes;
    public DataDescriptor Data;
    public ComparisonDescriptor ComparisonFlags;
    public CorLibTypeDescriptor CorLibTypeDescriptor;
    public ExceptionHandlerDescriptor ExceptionHandlers;
    public HashDescriptor HashDescriptor;
}