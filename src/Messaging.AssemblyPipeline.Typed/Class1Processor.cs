using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline.Typed
{
    public class Class1Processor : IMiddleware<Context>
    {
        public Task<Context> InvokeAsync(Context context, MiddlewareDelegate<Context> next)
        {
            if (context.Request is Class1)
            {
                context.Response = new Class3();
            }

            return next(context);
        }
    }
}
