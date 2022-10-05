using System;
using System.Threading;

namespace Threads
{
    internal class ThreadPoolWorker<TResult>
    {
        private readonly Func<object, TResult> func;
        private TResult result;

        public ThreadPoolWorker(Func<object, TResult> func)
        {
            this.func = func ?? throw new ArgumentException(nameof(func));
        }

        public bool Success { get; private set; } = false;
        public bool Completed { get; private set; } = false;
        public Exception Exeption { get; private set; } = null;
        public TResult Result
        {
            get
            {
                while (Completed == false)
                {
                    Thread.Sleep(150);
                }
                return Success == true && Exeption == null ? result : throw Exeption;
            }
        }

        public void Start(object state)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadExecution), state);
        }

        private void ThreadExecution(object state)
        {
            try
            {
                func.Invoke(state);
                Success = true;
            }
            catch (Exception ex)
            {
                Exeption = ex;
                Success = false;
            }
            finally
            {
                Completed = true;
            }
        }
    }
}
