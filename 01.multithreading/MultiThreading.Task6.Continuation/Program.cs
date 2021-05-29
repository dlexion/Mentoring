/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    public class Program
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

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            int scenario = ChooseScenarioFromConsoleInput();

            var startTask = Task.Run(() =>
            {
                Console.WriteLine($"Start task | ThreadId: {Thread.CurrentThread.ManagedThreadId} | ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}");
                switch (scenario)
                {
                    case 2:
                        throw new Exception("Failed task");
                    case 3:
                        cancellationTokenSource.Cancel();
                        token.ThrowIfCancellationRequested();
                        return Task.CompletedTask;
                    default:
                        return Task.CompletedTask;
                }
            }, token);

            startTask.ContinueWith((x) =>
            {
                Console.WriteLine("Continuation task should be executed regardless of the result of the parent task.");
            });

            startTask.ContinueWith(x =>
            {
                Console.WriteLine(
                    "Continuation task should be executed when the parent task finished without success.");
            }, TaskContinuationOptions.NotOnRanToCompletion);

            startTask.ContinueWith(x =>
            {
                Console.WriteLine(
                    "Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation."
                    + $"| ThreadId: {Thread.CurrentThread.ManagedThreadId} | ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            startTask.ContinueWith(x =>
            {
                Console.WriteLine(
                    "Continuation task should be executed outside of the thread pool when the parent task would be cancelled."
                    + $"| ThreadId: {Thread.CurrentThread.ManagedThreadId} | ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}");
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            Console.ReadLine();
        }

        public static int ChooseScenarioFromConsoleInput()
        {
            var repeat = true;
            var scenario = 0;

            Console.WriteLine("------------------------------");
            do
            {
                Console.WriteLine("Choose scenario: 1 - task finishes with success; 2 - with fail; 3 - cancelled");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                    case "2":
                    case "3":
                        scenario = int.Parse(input);
                        repeat = false;
                        break;
                    default:
                        Console.WriteLine("Please enter valid value");
                        break;
                }


            } while (repeat);
            Console.WriteLine("------------------------------");

            return scenario;
        }
    }
}
