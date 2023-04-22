/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    public class Program
    {
        private const int _taskAmount = 100;
        private const int _maxIterationsCount = 1000;

        public static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();
            
            var tasks = CreateTaskArray(_taskAmount, _maxIterationsCount);
            Task.WaitAll(tasks);

            Console.ReadLine();
        }

        private static Task[] CreateTaskArray(int taskArrayLength, int maxIterationsCount)
        {
            var tasks = new Task[taskArrayLength];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew((iteration) => 
                {
                    int? taskNumber = iteration as int?;
                    if (taskNumber.HasValue)
                    {
                        for (int j = 1; j <= maxIterationsCount; j++)
                        {
                            Output(taskNumber.Value, j);
                        }
                    }
                }, i);
            }

            return tasks;
        }

        private static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
