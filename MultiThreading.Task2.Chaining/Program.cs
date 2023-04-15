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

            // feel free to add your code
            RandomArrayAverage();

            Console.ReadLine();
        }

        static void RandomArrayAverage()
        {
            var result = Task.Run(() =>
            {
                Random rnd = new Random();

                int arrayLength = 10;
                int[] array = new int[arrayLength];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = rnd.Next(100);
                }

                PrintResult(1, string.Join(", ", array));

                return array;
            })
            .ContinueWith((antecedent, obj) =>
            {
                int? taskNumber = obj as int?;
                int[] array = antecedent.Result;

                Random rnd = new Random();
                int randomInteger = rnd.Next(100);

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] *= randomInteger;
                }

                PrintResult(taskNumber.Value, string.Join(", ", array));

                return array;
            }, 2, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((antecedent, obj) =>
            {
                int? taskNumber = obj as int?;
                int[] array = antecedent.Result;
                Array.Sort(array);

                PrintResult(taskNumber.Value, string.Join(", ", array));

                return array;
            }, 3, TaskContinuationOptions.OnlyOnRanToCompletion)
            .ContinueWith((antecedent, obj) =>
            {
                int? taskNumber = obj as int?;
                int[] array = antecedent.Result;

                double average = Queryable.Average(array.AsQueryable());
                PrintResult(taskNumber.Value, average.ToString());

                return average;
            }, 4, TaskContinuationOptions.OnlyOnRanToCompletion);

            try
            {
                result.Wait();
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
           
        }

        static void PrintResult(int taskNumber, string result)
        {
            Console.WriteLine($"Task #{taskNumber} – {result}");
        }
    }
}
