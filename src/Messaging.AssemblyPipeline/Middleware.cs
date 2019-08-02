using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{
    public abstract class Middleware<TContext> : IMiddleware<TContext>
    {

        protected IMiddleware<TContext> Next { get; }

        protected Middleware(IMiddleware<TContext> next)
        {
            Next = next;
        }

        public abstract Task<TContext> InvokeAsync(TContext context);

    }
}
