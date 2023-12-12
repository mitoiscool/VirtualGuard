namespace VirtualGuard;

public struct VirtualGuardSettings
{
    public string Version;
    public byte DebugMessageKey;
    
    public LicenseType License;
    
}

public enum LicenseType
{
    Lite,
    Plus
}