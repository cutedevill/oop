using System.Threading;

namespace Threads
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine($"Id потока метода Main - {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("Для запуска нажмите любую клавишу");
            Console.ReadKey();

            ThreadPoolWorker threadPoolWorker = new ThreadPoolWorker(new Action<object>(StarWriter));
            threadPoolWorker.Start('^');

            for (int i = 0; i < 120; i++)
            {
                Console.Write('_');
                Thread.Sleep(50);
            }

            threadPoolWorker.Wait();

            Console.WriteLine($"\nМетод Main закончил свою работу.");

        }

        private static void StarWriter(object arg)
        {
            char item = (char)arg;

            for (int i = 0; i < 120; i++)
            {
                Console.Write(item);
                Thread.Sleep(50);
            }
        }

    }
}