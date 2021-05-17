/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var createArray = Task.Run(() =>
            {
                Console.WriteLine("First Task – creates an array of 10 random integer.");
                var arr = CreateArray(10);
                PrintArray(arr);
                return arr;
            });

            var multiplyArray = createArray.ContinueWith(x =>
            {
                Console.WriteLine("Second Task – multiplies this array with another random integer.");
                var multiplier = GetRandomInt();
                Console.WriteLine($"Multiplier: {multiplier}");
                var arr = MultiplyArray(x.Result, multiplier);
                PrintArray(arr);
                return arr;
            });

            var sortArray = multiplyArray.ContinueWith(x =>
            {
                Console.WriteLine("Third Task – sorts this array by ascending.");
                Array.Sort(x.Result);
                PrintArray(x.Result);
                return x.Result;
            });

            var avgValue = sortArray.ContinueWith(x =>
            {
                Console.WriteLine("Fourth Task – calculates the average value.");
                var avg = x.Result.Average();
                Console.WriteLine(avg);
                return avg;
            });

            Console.ReadLine();
        }

        public static int[] CreateArray(int numberOfElements)
        {
            return Enumerable.Range(0, numberOfElements).Select(_ => GetRandomInt()).ToArray();
        }

        public static int[] MultiplyArray(int[] arr, int multiplier)
        {
            return arr.Select(x => x * multiplier).ToArray();
        }

        public static void PrintArray(int[] arr)
        {
            Console.WriteLine($"[{string.Join(", ", arr)}]");
        }

        public static int GetRandomInt()
        {
            Random rnd = new Random();
            return rnd.Next(100);
        }
    }
}
