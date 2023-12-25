using System;

namespace NetTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("enter password:");
            if (Console.ReadLine() == "oliver")
            {
                Console.WriteLine("correct!");
            }
            else
            {
                Console.WriteLine("incorrect");
            }

            Console.ReadKey();
        }
    }
}