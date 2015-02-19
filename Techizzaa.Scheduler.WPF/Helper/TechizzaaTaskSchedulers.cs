using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Techizzaa
{
    public class TechizzaaTaskSchedulers : TaskScheduler
    {
        private BlockingCollection<Task> _taskCollection = new BlockingCollection<Task>();
        private Thread techizzaaTaskThread = null;

        public TechizzaaTaskSchedulers()
        {
            techizzaaTaskThread = new Thread(TechizzazMainThread);
            techizzaaTaskThread.Name = "TechizzaaSingleThread";
            techizzaaTaskThread.Start();
        }

        public override int MaximumConcurrencyLevel
        {
            get
            {
                   return 1;
            }
        }
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _taskCollection.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            //Asking scheduling first task
            _taskCollection.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (!taskWasPreviouslyQueued && Thread.CurrentThread==techizzaaTaskThread)
            {
                return TryExecuteTask(task);
            }
            return false;
        }

        private void TechizzazMainThread()
        {
            while (true)
            {
                Task internalTask = _taskCollection.Take();
                TryExecuteTask(internalTask);
            }
        }
    }
}
