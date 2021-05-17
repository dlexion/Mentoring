/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            //todo add demonstration
            var startTask = Task.Run(() =>
            {
                Console.WriteLine("Start task");
            });

            var aTask = startTask.ContinueWith((x) =>
            {
                Console.WriteLine("Continuation task should be executed regardless of the result of the parent task.");
            });

            var bTask = startTask.ContinueWith(x =>
            {
                Console.WriteLine(
                    "Continuation task should be executed when the parent task finished without success.");
            }, TaskContinuationOptions.NotOnRanToCompletion);

            var cTask = startTask.ContinueWith(x =>
            {
                Console.WriteLine(
                    "Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            var dTask = startTask.ContinueWith(x =>
            {
                Console.WriteLine(
                    "Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.RunContinuationsAsynchronously);

            Console.ReadLine();
        }
    }
}
