using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSchedul;

namespace oop_2
{
    internal class main
    {
        public static void Main()
        {
            Console.WriteLine($"Main running with id {Thread.CurrentThread.ManagedThreadId}");

            Task[] tasks = new Task[5];
            Task_Scheduler task_scheduler = new Task_Scheduler();

            QueueTaskTesting(tasks, task_scheduler);
            TryExecuteTaskInlineTesting(tasks, task_scheduler);
            TryDequeueTesting(tasks, task_scheduler);

            try
            {
                Task.WaitAll(tasks);
            }
            catch
            {
                Console.WriteLine("Some tasks are canceled");
            }
            finally
            {
                Console.WriteLine("Main finished executing");
            }

            Console.ReadKey();
        }


        static void QueueTaskTesting(Task[] tasks, Task_Scheduler task_scheduler)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task<int>(() =>
                {
                    Thread.Sleep(1000 + i * 100);
                    Console.WriteLine($"The task {Task.CurrentId} worked in the {Thread.CurrentThread.ManagedThreadId} thread");
                    return 1;
                });
                tasks[i].Start(task_scheduler);
            }
        }

        static void TryExecuteTaskInlineTesting(Task[] tasks, Task_Scheduler task_scheduler)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task<int>(() =>
                {
                    Thread.Sleep(1000 + i * 100);
                    Console.WriteLine($"The task {Task.CurrentId} worked in the {Thread.CurrentThread.ManagedThreadId} thread");
                    return 1;
                });
            }

            foreach (var task in tasks)
            {
                task.Start(task_scheduler);
                task.Wait();
            }
        }

        static void TryDequeueTesting(Task[] tasks, Task_Scheduler task_scheduler)
        {
            CancellationTokenSource cancellation_token_source = new CancellationTokenSource();

            CancellationToken cancellationToken = cancellation_token_source.Token;

            cancellation_token_source.CancelAfter(666);

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Thread.Sleep(1000 + i * 100);
                    Console.WriteLine($"The task {Task.CurrentId} worked in the {Thread.CurrentThread.ManagedThreadId} thread");
                }, cancellationToken);
            }

        }
    }
}
