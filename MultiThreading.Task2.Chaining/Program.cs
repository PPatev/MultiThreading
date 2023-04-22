/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    public class Program
    {
        private static readonly int _arrayLength = 10;

        private static int _taskNumber = default;

        public static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code
            try
            {
                var task = RandomArrayAverage();
                task.Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                {
                    if (innerEx is OverflowException)
                    {
                        Console.WriteLine(innerEx.Message);
                    }
                    else
                    {
                        throw innerEx;
                    }
                }
            }

            Console.ReadLine();
        }

        private static Task RandomArrayAverage()
        {
            var task = Task.Run(() => CreateRandomIntegerArray())
            .ContinueWith((antecedent) => MultiplyArrayWithRandomInteger(antecedent), TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((antecedent) => SortArrayAscending(antecedent), TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((antecedent) => CalculateArrayAverage(antecedent), TaskContinuationOptions.OnlyOnRanToCompletion);

            return task;
        }

        private static int[] CreateRandomIntegerArray()
        {
            var rnd = new Random();

            var array = new int[_arrayLength];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(0, 100);
            }

            Interlocked.Increment(ref _taskNumber);
            PrintTaskResult(string.Join(", ", array));

            return array;
        }

        private static int[] MultiplyArrayWithRandomInteger(Task<int[]> antecedent)
        {
            var array = antecedent.Result;

            var rnd = new Random();
            var randomInteger = rnd.Next(100);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] *= randomInteger;
            }

            Interlocked.Increment(ref _taskNumber);
            PrintTaskResult(string.Join(", ", array));

            return array;
        }

        private static int[] SortArrayAscending(Task<int[]> antecedent)
        {
            var array = antecedent.Result;
            Array.Sort(array);

            Interlocked.Increment(ref _taskNumber);
            PrintTaskResult(string.Join(", ", array));

            return array;
        }

        private static double CalculateArrayAverage(Task<int[]> antecedent)
        {
            var array = antecedent.Result;
            var average = Queryable.Average(array.AsQueryable());

            Interlocked.Increment(ref _taskNumber);
            PrintTaskResult(average.ToString());

            return average;
        }

        private static void PrintTaskResult(string result)
        {
            Console.WriteLine($"Task #{_taskNumber} – {result}");
        }
    }
}
