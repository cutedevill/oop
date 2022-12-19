using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;



namespace TaskSchedul
{
    internal class Task_Scheduler : TaskScheduler
    {
        private readonly LinkedList<Task> task_list = new LinkedList<Task>();

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return task_list;
        }

        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"Task {task.Id} is in queue");
            task_list.AddLast(task);
            ThreadPool.QueueUserWorkItem(ExecuteTasks, null);
            AddToTheTop(task);
            AddToTheEnd(task);
        }


        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine($"Synchronous task execution\r\n {task.Id}");

            lock (task_list)
            {
                task_list.Remove(task);
            }

            return base.TryExecuteTask(task);
        }

        protected override bool TryDequeue(Task task)
        {
            Console.WriteLine($"Deleting the task {task.Id}");

            bool res = false;

            lock (task_list)
            {
                res = task_list.Remove(task);
            }

            if (res)
            {
                Console.WriteLine($"The task {task.Id} was successfully deleted");
            }

            return res;
        }

        private void ExecuteTasks(object _)
        {
            while (true)
            {
                Task task = null;

                lock (task_list)
                {
                    if (task_list.Count == 0)
                        break;

                    task = task_list.First.Value;
                    task_list.RemoveFirst();
                }

                if (task == null)
                    break;

                base.TryExecuteTask(task);
            }
        }

        private void AddToTheEnd(Task task)
        {
            var current = task_list.FirstOrDefault(x => x.Id == task.Id);

            if (current != null)
            {
                lock (task_list)
                {
                    task_list.AddLast(current);
                    task_list.Remove(task_list.FirstOrDefault(x => x.Id == task.Id));
                    Console.WriteLine($"The task {task.Id} was successfully added to the end");
                }
            }
        }

        private void AddToTheTop(Task task)
        {
            var current = task_list.FirstOrDefault(x => x.Id == task.Id);

            if (current != null)
            {
                lock (task_list)
                {
                    task_list.Remove(task_list.FirstOrDefault(x => x.Id == task.Id));
                    task_list.AddFirst(current);
                    Console.WriteLine($"The task {task.Id} was successfully added to the top");
                }
            }
        }
    }
}

