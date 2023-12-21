namespace VirtualGuard.Runtime.Dynamic
{

    public static class Constants
    {
        // reader
        public static byte HEADER_IV = 0;
        
        public static byte HEADER_ROTATION_FACTOR1 = 0;
        public static byte HEADER_ROTATION_FACTOR2 = 0;
        public static byte HEADER_ROTATION_FACTOR3 = 0;
        
        public static byte HANDLER_ROT1 = 0;
        public static byte BYTE_ROT1 = 0;

        public static byte HANDLER_ROT2 = 0;
        public static byte BYTE_ROT2 = 0;
        
        public static byte HANDLER_ROT3 = 0;
        public static byte BYTE_ROT3 = 0;
        
        public static byte HANDLER_ROT4 = 0;
        public static byte BYTE_ROT4 = 0;
        
        public static byte HANDLER_ROT5 = 0;
        public static byte BYTE_ROT5 = 0;
        
        // data
        public static string DT_WATERMARK;
        public static string DT_NAME;
        
        // flags

        public static byte CMP_GT;
        public static byte CMP_LT;
        public static byte CMP_EQ;
        
        public static int CorlibID_I;
        public static int CorlibID_I1;
        public static int CorlibID_I2;
        public static int CorlibID_I4;
        public static int CorlibID_I8;
        
        public static int CorlibID_U;
        public static int CorlibID_U1;
        public static int CorlibID_U2;
        public static int CorlibID_U4;
        public static int CorlibID_U8;

        public static byte CatchFL;
        public static byte FaultFL;
        public static byte FilterFL;
        public static byte FinallyFL;
    }
}