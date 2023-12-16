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

        public static void PrintDebug(string message)
        { // this will be removed if built in release
            Console.WriteLine(message);
        }

    }
}