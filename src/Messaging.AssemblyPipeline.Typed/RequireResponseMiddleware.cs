using System;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline.Typed
{
    public class RequireResponseMiddleware : Middleware<Context>
    {
        public RequireResponseMiddleware(IMiddleware<Context> next) : base(next)
        {
        }

        public override Task<Context> InvokeAsync(Context context)
        {
            if (context.Response == null)
                throw new InvalidOperationException("No response from any middleware.");

            return Next.InvokeAsync(context);
        }
    }
}
