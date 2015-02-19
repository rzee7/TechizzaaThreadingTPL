using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Techizzaa
{
    class Program
    {
        static void Main(string[] args)
        {

            //TechizzaaThreadHolder.ExecuteMainThread(true);
            //TechizzaaThreadHolder.ExecuteIteratorThread(5);
            //TechizzaaThreadHolder.CallParallel();
            //TechizzaaThreadHolder.CallWithoutParallel(true);
            //CallExtra();  //BlockingCollection
            Console.Read();

        }

        #region Producers

        static void CallExtra()
        {
            var queue = new BlockingCollection<int>();

            var producers = Enumerable.Range(1, 3).Select(_ =>

                Task.Factory.StartNew(() =>
                {
                    Enumerable.Range(1, 100).ToList().ForEach(i =>
                    {
                        queue.Add(i);
                        Thread.Sleep(100);
                    });
                })).ToArray();


            var cconsumers = Enumerable.Range(1, 2).Select(_ =>

               Task.Factory.StartNew(() =>
               {
                   foreach (var item in queue.GetConsumingEnumerable())
                   {
                       Console.WriteLine(item);
                   }
               })).ToArray();

            Task.WaitAll(producers);
            queue.CompleteAdding();
            //queue.Add(1000);
            Task.WaitAll(cconsumers);
        }

        #endregion
    }

    public class TechizzaaThreadHolder
    {
        #region Main Thread

        public static void ExecuteMainThread(bool isChildThread)
        {
            //TODO: Load state from previously suspended application

            ThreadStart childThreadRef = new ThreadStart(DestroyThread);//CallToChildThread);
            var mainThread = Thread.CurrentThread;
            mainThread.Name = "Main Thread";
            Console.WriteLine("Main Thread Name : " + mainThread.Name);
            Thread childThread = new Thread(childThreadRef);
            childThread.Name = "ChhotuMotu";
            childThread.Start();
            Thread.Sleep(2000);
            //now abort the child
            Console.WriteLine("In Main: Aborting the Child thread");
            childThread.Abort();
            Console.ReadKey();
        }

        private static void CallToChildThread()
        {
            Console.WriteLine("Child thread starts");
            Console.WriteLine("Child Thread Paused for {0} seconds", TechConstants.SleepTimeInterval / 1000);
            Thread.Sleep(TechConstants.SleepTimeInterval);
            Console.WriteLine("Child thread resumes");
        }

        public static void DestroyThread()
        {
            try
            {

                Task t = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Task created");
                });


                Console.WriteLine("Child thread starts");
                // do some work, like counting to 10
                for (int counter = 0; counter <= 10; counter++)
                {
                    Thread.Sleep(500);
                    Console.WriteLine(counter);
                }
                Console.WriteLine("Child Thread Completed");

            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine("Thread Abort Exception");
            }
            finally
            {
                Console.WriteLine("Couldn't catch the Thread Exception");
            }

        }

        #endregion

        #region Using Thread For Iterations

        public static void ExecuteIteratorThread(int ThreadNumber)
        {
            for (int i = 0; i < ThreadNumber; i++)
            {
                Thread techizzaaThread = new Thread(LongItaration);
                techizzaaThread.Name = TechConstants.ThreadName + "_" + ThreadNumber; // Provide Thread Name to identify
                techizzaaThread.Start(); //Starting Thread
                Console.WriteLine("Executing Thread on : " + techizzaaThread.Name);
            }
        }

        #endregion

        #region Long Iteration Method

        private static void LongItaration()
        {
            string x = string.Empty;
            for (int techVal = 0; techVal < TechConstants.IterationValue; techVal++)
            {
                x = x + "s";
            }
        }

        #endregion

        #region Using Parallel Library

        public static void CallParallel()
        {
            Parallel.For(0, TechConstants.IterationValue, x => LongItaration());
        }

        #endregion

        #region With and Without Parallel

        public static void CallWithoutParallel(bool isParallel)
        {
            Action<Action> measure = (body) =>
            {
                var startTime = DateTime.Now;
                body();
                Console.WriteLine("Time Execution : {0} THread ID : {1}", DateTime.Now - startTime, Thread.CurrentThread.ManagedThreadId);
            };

            Action calJob = () =>
            {
                for (int i = 0; i < TechConstants.IterationValue; i++) ;
            };
            Action job = () => { Thread.Sleep(1000); };

            //run sequentially
            if (!isParallel)
                Enumerable.Range(1, 100).ToList().ForEach(_ => measure(calJob));
            else
                Enumerable.Range(1, 100).AsParallel().WithDegreeOfParallelism(100).ForAll(_ => measure(calJob));
        }

        #endregion
    }
}
