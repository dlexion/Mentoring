/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("a) Use Thread class for this task and Join for waiting threads.");
            var thread = new Thread(() => ThreadRecursion(10));
            thread.Start();
            thread.Join();

            Console.WriteLine("b) ThreadPool class for this task and Semaphore for waiting threads.");
            ThreadPool.SetMinThreads(10, 10);
            ThreadPool.QueueUserWorkItem(_ => ThreadPoolRecursion(10));

            Console.ReadLine();
        }

        public static void ThreadRecursion(int state)
        {
            Console.WriteLine($"State: {state}");
            Console.WriteLine($"Current thread id: {Thread.CurrentThread.ManagedThreadId}");
            if (Interlocked.Decrement(ref state) == 0)
            {
                return;
            }

            var thread = new Thread(() => ThreadRecursion(state));
            thread.Start();
            thread.Join();
        }

        public static void ThreadPoolRecursion(int state)
        {
            _semaphore.Wait();
            Console.WriteLine($"State: {state}");
            Console.WriteLine($"Current thread id: {Thread.CurrentThread.ManagedThreadId}");
            if (Interlocked.Decrement(ref state) == 0)
            {
                return;
            }
            ThreadPool.QueueUserWorkItem(_ => ThreadPoolRecursion(state));

            _semaphore.Release();
        }
    }
}
