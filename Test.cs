using System;

namespace Test
{
    class Program1
    {
        static void Method1()
        {
            Console.WriteLine("This is a test");

            Console.WriteLine("Random");
        }
        
        static void Method2()
        {
            Console.WriteLine("This is a test");

            void Test()
            {
                Console.WriteLine("This is a test");
            }

            Console.WriteLine("Random");
        }
    }

    class Program2
    {
        static void Method1()
        {
            Console.WriteLine("This is a test");

            Console.WriteLine("Random");
        }

        static void Method2()
        {

        }
    }
}