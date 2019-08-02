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

            var pipeline = new AssemblyPipeline<string>();
            
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {      
                pipeline.WithMiddleware(new IncrementMiddleware());
            }

            sw.Stop();

            Console.WriteLine("Adding middleware took {0}", sw.ElapsedTicks);

            sw.Restart();

            _ = await pipeline.InvokeAsync("");

            sw.Stop();

            Console.WriteLine("Getting took {0}", sw.ElapsedTicks);

            pipeline.Dispose();            

            var pipeline2 = new AssemblyPipeline<string>();

            sw.Restart();

            for (int i = 0; i < 1000; i++)
            {
                pipeline2.WithMiddleware((context, next) => next.Invoke(context + "hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world"));
            }

            sw.Stop();

            Console.WriteLine("Adding middleware took {0}", sw.ElapsedTicks);

            sw.Restart();

            _ = await pipeline2.InvokeAsync("");

            sw.Stop();

            Console.WriteLine("Getting took {0}", sw.ElapsedTicks);

            pipeline2.Dispose();

            Console.ReadKey(false);
        }


        private class IncrementMiddleware : IMiddleware<string>
        {
            public async Task<string> InvokeAsync(string context, MiddlewareDelegate<string> next)
            {
                return await next.Invoke(context + "hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world hello world");
            }
        }

    }
}
