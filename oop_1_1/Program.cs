///5. Написать консольное приложение, которое выводит на экран символы в разных потоках. Использовать Thread. (по аналогии с примером из лекции)
using System.Threading;

namespace Threads
{
    internal class Program
    {
        private static void Main()
        {
            Thread thread = new Thread(WriteString);

            Console.WriteLine("Для запуска нажмите любую клавишу");
            Console.ReadKey();
            Console.Clear();

            thread.Start("k");

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("i");
            }
            Console.ReadLine();

        }

        private static void WriteString(object arg)
        {
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine($"\t{arg}");

            }
        }
    }
}


