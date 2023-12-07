namespace VirtualGuard.Runtime.Dynamic;

public static class Constants
{
    public static byte OP_ADD;
    public static byte OP_SUB;
    public static byte OP_MUL;
    public static byte OP_DIV;
    
    public static byte OP_LDLOC;
    public static byte OP_STLOC;
    
    public static byte OP_POP;
    public static byte OP_DUP;
    
    public static byte OP_LDELEM;
    public static byte OP_SETELEMENT;

    public static byte OP_LDC_I4;
    public static byte OP_LDC_I8;
    
    public static byte OP_LDFLD;
    public static byte OP_STFLD;

    // reader
    public static int RD_IV;
    public static int RD_HANDLER_ROT;
    public static int RD_BYTE_ROT;
    
    // data
    public static string DT_WATERMARK;
    
    
    // msg
    public static string MSG_INVALID;
    
}