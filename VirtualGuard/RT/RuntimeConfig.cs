namespace VirtualGuard.RT;

internal static class RuntimeConfig
{
    public const string RuntimePath = "VirtualGuard.Runtime.dll";
    
    public const string Constants = "VirtualGuard.Runtime.Dynamic.Constants";
    public const string Routines = "VirtualGuard.Runtime.Dynamic.Routines";
    
    public const string BaseHandler = "VirtualGuard.Runtime.OpCodes.impl";
    
    public const string VmEntry = "VirtualGuard.Runtime.Entry:VMEntryBasic";
    public const string VmEntryInst = "VirtualGuard.Runtime.Entry:VMEntryInst";
    public const string VmEntryNoArgs = "VirtualGuard.Runtime.Entry:VMEntryNoArgs";

    public const string JunkCode = "VirtualGuard.Runtime.Junk.HandlerJunkCode";
}