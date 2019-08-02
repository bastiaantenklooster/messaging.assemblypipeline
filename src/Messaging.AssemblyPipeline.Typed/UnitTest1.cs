using System;
using Xunit;

namespace Messaging.AssemblyPipeline.Typed
{
    public class UnitTest1
    {
        [Fact]
        public async System.Threading.Tasks.Task Test1()
        {
            var pipeline = new AssemblyPipeline<Context>();

            pipeline.WithMiddleware<RequireResponseMiddleware>();

            var responseMiddleware = new ResponseMiddleware();
            responseMiddleware.AddResponseType<Class1, Class3>();
            pipeline.WithMiddleware(responseMiddleware);

            pipeline.WithMiddleware<Class1Processor>();
            
            await pipeline.InvokeAsync(new Context(new Class1()));
            await pipeline.InvokeAsync(new Context(new Class2()));

        }
    }
}
