/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> numbers = new List<int>();
        private static int count = 1;
        private static int maxNumber = 10;
        private static EventWaitHandle handle =
            new EventWaitHandle(false, EventResetMode.AutoReset);
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code
            
            //Thread addThread = new Thread(AddNumbers);
            //addThread.Start();
            Thread printThread = new Thread(PrintNumbers);
            printThread.Start();
            

            Console.ReadLine();
        }

        static void AddNumbers()
        {
            while (count <= maxNumber) 
            {
                numbers.Add(count);
                handle.WaitOne();
                Interlocked.Increment(ref count);
            }
        }

        static void PrintNumbers()
        {
            Thread addThread = new Thread(AddNumbers);
            addThread.Start();
            while (count <= maxNumber) 
            {
                Console.WriteLine(string.Join(", ", numbers));
                handle.Set();
            }
        }
    }
}
