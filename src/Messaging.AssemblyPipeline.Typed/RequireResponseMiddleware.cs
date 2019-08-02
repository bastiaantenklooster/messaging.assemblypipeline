using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline.Typed
{
    public class RequireResponseMiddleware : IMiddleware<Context>
    {
        public Task<Context> InvokeAsync(Context context, MiddlewareDelegate<Context> next)
        {
            if (context.Response == null)
                throw new InvalidOperationException("No response from any middleware.");

            return next(context);
        }
    }
}
