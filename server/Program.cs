using System;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;

namespace server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var pipe = new NamedPipeServerStream("test", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
            {
                Console.WriteLine("Waiting... ");
                await pipe.WaitForConnectionAsync();
                Console.WriteLine("Connected? " + pipe.IsConnected);

                var shouldStop = false;
                Console.CancelKeyPress += delegate
                {
                    Console.WriteLine("Stopping...");
                    shouldStop = true;
                };

                using (var sr = new StreamReader(pipe))
                {
                    while (!shouldStop)
                    {
                        Console.WriteLine(await sr.ReadLineAsync());
                    }
                }

                if (pipe.IsConnected)
                {
                    Console.WriteLine("Draining...");
                    pipe.WaitForPipeDrain();
                }
            }
        }
    }
}
