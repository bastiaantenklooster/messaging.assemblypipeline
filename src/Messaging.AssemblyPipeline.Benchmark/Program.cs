using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline.Benchmark
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sw = new Stopwatch();

            var pipeline = new AssemblyPipeline<int>();

            sw.Start();

            for (var i = 0; i < 10000; i++)
            {
                pipeline.WithMiddleware<IncrementMiddleware>();
            }

            sw.Stop();

            Console.WriteLine("Adding middleware took {0}", sw.ElapsedTicks);

            sw.Restart();

            var res = await pipeline.InvokeAsync(0);

            sw.Stop();

            Console.WriteLine("Getting {1} took {0}", sw.ElapsedTicks, res);

            pipeline.Dispose();

            Console.ReadKey(false);
        }


        private class IncrementMiddleware : Middleware<int>
        {

            public IncrementMiddleware(IMiddleware<int> next) : base(next)
            {
            }

            public override Task<int> InvokeAsync(int context)
            {
                return Next.InvokeAsync(++context);
            }
        }

    }
}
