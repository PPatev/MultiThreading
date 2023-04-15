/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            //a
            var source = new CancellationTokenSource();
            var token = source.Token;

            Console.WriteLine("a) Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("Choose parent task state options:");
            Console.WriteLine("Is faulted -> 1");
            Console.WriteLine("Is cancelled -> 2");
            Console.WriteLine("Is completed successfully -> 3");
            var input = Console.ReadLine();
            if (int.TryParse(input, out var option) && option >= 1 && option <= 3)
            {
                if (option == 2)
                {
                    source.Cancel();
                }

                Task.Factory.StartNew((obj) =>
                {
                    if (token.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
                    
                    switch ((int)obj)
                    {
                        case 1:
                            Console.WriteLine("Parent task throws exception");
                            throw new InvalidOperationException();
                        case 3:
                            Console.WriteLine("Parent task completes successfully");
                            break;

                    }
                }, option, token)
                .ContinueWith((antecedent) =>
                {
                    Console.WriteLine($"Antecedent status: {antecedent.Status}");
                    Console.WriteLine("Continuation task successful");
                }, TaskContinuationOptions.None).Wait();
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();

            //b
            Console.WriteLine("b) Continuation task should be executed when the parent task finished without success.");
            Task.Factory.StartNew(() => 
            {
                Console.WriteLine("Parent task throws exception");
                throw new ArgumentException(); 
            })
            .ContinueWith((antecedent) =>
            {
                Console.WriteLine($"Antecedent status: {antecedent.Status}");
                Console.WriteLine("Continuation successful");
            }, TaskContinuationOptions.NotOnRanToCompletion).Wait();

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();

            //c
            Console.WriteLine("c) Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Parent task ManagedThreadId: {Thread.CurrentThread.ManagedThreadId}");
                throw new ArgumentException();
            })
            .ContinueWith((antecedent) =>
            {
                Console.WriteLine($"Antecedent status: {antecedent.Status}");
                Console.WriteLine($"Continuation task ManagedThreadId: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine("Continuation successful");
            }, TaskContinuationOptions.OnlyOnFaulted & TaskContinuationOptions.ExecuteSynchronously).Wait();

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();

            //d
            Console.WriteLine("d) Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
           
            var sourceD = new CancellationTokenSource();
            var tokenD = sourceD.Token;
            sourceD.Cancel();

            await Task.Run(() =>
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Parent task is cancelled");
                    throw new TaskCanceledException();
                }
            }, tokenD)
            .ContinueWith((antecedent) =>
            {
                Console.WriteLine($"Antecedent status: {antecedent.Status}");
                Console.WriteLine($"Continuation thread IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
                Console.WriteLine("Continuation successful");
            }, TaskContinuationOptions.RunContinuationsAsynchronously);


            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
