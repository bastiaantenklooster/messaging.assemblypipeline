using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline.Typed
{
    public class Class1Processor : Middleware<Context>
    {
        public Class1Processor(IMiddleware<Context> next) : base(next)
        {
        }

        public override Task<Context> InvokeAsync(Context context)
        {

            if (context.Request is Class1)
            {
                context.Response = new Class3();
            }

            return Next.InvokeAsync(context);
        }
    }
}
