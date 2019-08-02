using System;
using System.Threading.Tasks;
using Xunit;

namespace Messaging.AssemblyPipeline.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var pipeline = new AssemblyPipeline<int>();

            pipeline.WithMiddleware<IncrementDelayMiddleware>();
            pipeline.WithMiddleware(new IncrementDelayMiddleware());
            pipeline.WithMiddleware(new IncrementDelayMiddleware());
            pipeline.WithMiddleware((context, next) => next.Invoke(context+5));
            pipeline.WithMiddleware(new IncrementDelayMiddleware());

            var task1 = pipeline.InvokeAsync(0);

            pipeline.WithMiddleware(new IncrementDelayMiddleware());
            pipeline.WithMiddleware(new IncrementDelayMiddleware());
            pipeline.WithMiddleware(new IncrementDelayMiddleware());
            pipeline.WithMiddleware(new IncrementDelayMiddleware());
            pipeline.WithMiddleware((context, next) => next.Invoke(context + 3));

            var task2 = pipeline.InvokeAsync(0);

            await Task.WhenAll(task1, task2);

            Assert.Equal(9, task1.Result);
            Assert.Equal(16, task2.Result);

            pipeline.Dispose();
        }

        [Fact]
        public async Task Test2()
        {
            var pipeline = new AssemblyPipeline<int>();

            pipeline.WithMiddleware<PassthroughMiddleware>();
            pipeline.WithMiddleware(new DivideAfterMiddleware());
            pipeline.WithMiddleware<PassthroughMiddleware>();
            pipeline.WithMiddleware(new MultiplyBeforeMiddleware());
            pipeline.WithMiddleware(new MultiplyAfterMiddleware());
            pipeline.WithMiddleware<PassthroughMiddleware>();
            pipeline.WithMiddleware(new DivideAfterMiddleware());
            pipeline.WithMiddleware(new PassthroughMiddleware());

            Assert.Equal(10, await pipeline.InvokeAsync(5));

            pipeline.Dispose();
        }


        private class IncrementDelayMiddleware : IMiddleware<int>
        {

            public async Task<int> InvokeAsync(int context, MiddlewareDelegate<int> next)
            {
                var result = await next.Invoke(++context);

                await Task.Delay(context * 10);

                return result;
            }

        }

        private class MultiplyBeforeMiddleware : IMiddleware<int>
        {

            public async Task<int> InvokeAsync(int context, MiddlewareDelegate<int> next)
            {
                context *= 3;

                return await next.Invoke(context);
            }

        }

        private class DivideAfterMiddleware : IMiddleware<int>
        {

            public async Task<int> InvokeAsync(int context, MiddlewareDelegate<int> next)
            { 
                var result = await next.Invoke(context);

                return result / 2;
            }

        }

        private class MultiplyAfterMiddleware : IMiddleware<int>
        {

            public async Task<int> InvokeAsync(int context, MiddlewareDelegate<int> next)
            {
                var result = await next.Invoke(context);

                return result * 3;
            }

        }

        private class PassthroughMiddleware : IMiddleware<int>
        {

            public async Task<int> InvokeAsync(int context, MiddlewareDelegate<int> next)
            {
                return await next.Invoke(context);
            }

        }

    }
}
