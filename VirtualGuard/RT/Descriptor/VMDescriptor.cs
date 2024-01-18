using VirtualGuard.RT.Descriptor.Handler;

namespace VirtualGuard.RT.Descriptor;

internal class VMDescriptor
{
    public HandlerResolver OpCodes;
    public DataDescriptor Data;
    public ComparisonDescriptor ComparisonFlags;
    public CorLibTypeDescriptor CorLibTypeDescriptor;
    public ExceptionHandlerDescriptor ExceptionHandlers;
    public HashDescriptor HashDescriptor;
}