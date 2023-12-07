namespace VirtualGuard.RT.Descriptor;

public class EncryptionDescriptor
{
    public EncryptionDescriptor()
    {
        var rnd = new Random();
        RD_IV = (byte)rnd.Next(255);
        RD_HANDLER_ROT = (byte)rnd.Next(255);
        RD_BYTE_ROT = (byte)rnd.Next(255);
    }

    public static byte RD_IV;
    public static byte RD_HANDLER_ROT;
    public static byte RD_BYTE_ROT;
    
    
}