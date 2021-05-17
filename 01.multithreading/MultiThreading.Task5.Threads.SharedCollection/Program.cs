/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{

    class Program
    {
        //private static ConcurrentQueue<int> _queue = new ConcurrentQueue<int>();
        private static Queue<int> _queue = new Queue<int>();
        private static AutoResetEvent _waitAddHandle = new AutoResetEvent(false);
        private static AutoResetEvent _waitPrintHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();


            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var addTask = Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    _queue.Enqueue(i);
                    _waitAddHandle.Set();
                    _waitPrintHandle.WaitOne();
                }
                cts.Cancel();
                _waitAddHandle.Set();
            });

            var printTask = Task.Run(() =>
            {
                while (true)
                {
                    _waitAddHandle.WaitOne();
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    PrintArray(_queue.ToArray());
                    _waitPrintHandle.Set();
                }
            }, token);

            Task.WaitAll(addTask, printTask);

            Console.ReadLine();
        }

        public static void PrintArray(int[] arr)
        {
            Console.WriteLine($"[{string.Join(", ", arr)}]");
        }
    }
}
