/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    public class Program
    {
        private static List<int> _numbers = new List<int>();
        private static int _count = 1;
        private static int _maxNumber = 10;

        private static EventWaitHandle _firstHandle = new AutoResetEvent(false);
        private static EventWaitHandle _secondHandle = new AutoResetEvent(true);

        private static object _lock = new object();

        public static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code

            var addThread = new Thread(AddNumbers);
            addThread.Start();
            var printThread = new Thread(PrintNumbers);
            printThread.Start();

            Console.ReadLine();
        }

        private static void AddNumbers()
        {
            while (_count <= _maxNumber) 
            {
                _secondHandle.WaitOne();
                lock (_lock) 
                {
                    _numbers.Add(_count);
                }
                
                _firstHandle.Set();
                Interlocked.Increment(ref _count);
            }
        }

        private static void PrintNumbers()
        {
            while (_count <= _maxNumber) 
            {
                _firstHandle.WaitOne();
                lock (_lock)
                {
                    Console.WriteLine(string.Join(", ", _numbers));
                }
                _secondHandle.Set();
            }
        }
    }
}
