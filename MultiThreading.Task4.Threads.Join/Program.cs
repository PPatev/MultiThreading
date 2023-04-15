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
        private static Semaphore _pool;
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // feel free to add your code
            Thread thread = new Thread(new ParameterizedThreadStart(DecrementNumber));
            thread.Start(10);
            thread.Join();

            _pool = new Semaphore(initialCount: 0, maximumCount: 1);
            ThreadPool.QueueUserWorkItem(DecrementNumberSemaphore, 10);
            _pool.Release(releaseCount: 1 );

            Console.ReadLine();
        }

        static void DecrementNumber(object data)
        {
            int iteration = (int)data;
            if (iteration == 0)
            {
                return;
            }

            Console.WriteLine(iteration);
            Interlocked.Decrement(ref iteration);

            Thread thread = new Thread(new ParameterizedThreadStart(DecrementNumber));
            thread.Start(iteration);
            thread.Join();
        }

        static void DecrementNumberSemaphore(object data)
        {
            _pool.WaitOne();
            int iteration = (int)data;
            if (iteration == 0)
            {
                _pool.Release();
                return;
            }

            Console.WriteLine(iteration);
            Interlocked.Decrement(ref iteration);
            _pool.Release();

            ThreadPool.QueueUserWorkItem(DecrementNumberSemaphore, iteration);
        }
    }
}
