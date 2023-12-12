namespace VirtualGuard.Runtime.Dynamic
{

    public static class Routines
    {
        public static void Exit(string msg)
        {
            // dynamically replaced
        }

        public static string EncryptDebugMessage(string message)
        { // this will just mark a string for encryption
            return null;
        }

    }
}