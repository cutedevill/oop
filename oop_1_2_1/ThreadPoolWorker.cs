using System.Threading;

namespace Threads
{
    internal class ThreadPoolWorker
    {
        private readonly Action<object> action;

        public ThreadPoolWorker(Action<object> action)
        {
            this.action = action ?? throw new ArgumentException(nameof(action));
        }

        public bool Success { get; private set; } = false;
        public bool Completed { get; private set; } = false;
        public Exception Exeption { get; private set; } = null;

        public void Start(object state)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadExecution), state);
        }

        public void Wait()
        {
            while (Completed == false)
            {
                Thread.Sleep(150);
            }

            if(Exeption != null)
            {
                throw Exeption;
            }
        }

        private void ThreadExecution(object state)
        {
            try
            {
                action.Invoke(state);
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
