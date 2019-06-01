using System;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.IO;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await SendInfoToLocalPipe();
        }

        private static async Task SendInfoToLocalPipe()
        {
            using (var pipe = new NamedPipeClientStream(".", "test", PipeDirection.InOut))
            {
                Console.WriteLine("Connecting...");
                await pipe.ConnectAsync(3000);
                Console.WriteLine("Conected.");

                int i = 0;
                using (var sw = new StreamWriter(pipe))
                {
                    sw.AutoFlush = true;
                    while (Console.ReadKey(false).Key != ConsoleKey.Escape)
                    {
                        await sw.WriteLineAsync("Test " + ++i);
                    }
                }
            }
        }
    }
}
