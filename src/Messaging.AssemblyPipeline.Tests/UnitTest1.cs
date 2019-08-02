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
            pipeline.WithMiddleware<IncrementDelayMiddleware>();
            pipeline.WithMiddleware<IncrementDelayMiddleware>();
            pipeline.WithMiddleware((context, next) => next.InvokeAsync(context + 5));
            pipeline.WithMiddleware<IncrementDelayMiddleware>();

            var task1 = pipeline.InvokeAsync(0);

            pipeline.WithMiddleware<IncrementDelayMiddleware>();
            pipeline.WithMiddleware<IncrementDelayMiddleware>();
            pipeline.WithMiddleware<IncrementDelayMiddleware>();
            pipeline.WithMiddleware<IncrementDelayMiddleware>();
            pipeline.WithMiddleware((context, next) => next.InvokeAsync(context + 3));

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

            pipeline.WithMiddleware(new PassthroughMiddleware());
            pipeline.WithMiddleware(new DivideAfterMiddleware());
            pipeline.WithMiddleware(new PassthroughMiddleware());
            pipeline.WithMiddleware(new MultiplyBeforeMiddleware());
            pipeline.WithMiddleware<MultiplyAfterMiddleware>();
            pipeline.WithMiddleware(new PassthroughMiddleware());
            pipeline.WithMiddleware(new DivideAfterMiddleware());
            pipeline.WithMiddleware(new PassthroughMiddleware());

            Assert.Equal(10, await pipeline.InvokeAsync(5));

            pipeline.Dispose();
        }


        private class IncrementDelayMiddleware : Middleware<int>
        {
            public IncrementDelayMiddleware(IMiddleware<int> next) : base(next)
            {
            }

            public override async Task<int> InvokeAsync(int context)
            {
                var result = await Next.InvokeAsync(++context);

                await Task.Delay(context * 10);

                return result;
            }
        }

        private class MultiplyBeforeMiddleware : IInstanceMiddleware<int>
        {

            public async Task<int> InvokeAsync(int context, IMiddleware<int> next)
            {
                context *= 3;

                return await next.InvokeAsync(context);
            }

        }

        private class DivideAfterMiddleware : IInstanceMiddleware<int>
        {

            public async Task<int> InvokeAsync(int context, IMiddleware<int> next)
            {
                var result = await next.InvokeAsync(context);

                return result / 2;
            }

        }

        private class MultiplyAfterMiddleware : Middleware<int>
        {
            public MultiplyAfterMiddleware(IMiddleware<int> next) : base(next)
            {
            }

            public override async Task<int> InvokeAsync(int context)
            {
                var result = await Next.InvokeAsync(context);

                return result * 3;
            }

        }

        private class PassthroughMiddleware : IInstanceMiddleware<int>
        {

            public async Task<int> InvokeAsync(int context, IMiddleware<int> next)
            {
                return await next.InvokeAsync(context);
            }

        }

    }
}
