using System;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{
    public class AnonymousMiddleware<TContext> : IMiddleware<TContext>
    {

        public IMiddleware<TContext> Next { get; }

        public Func<TContext, IMiddleware<TContext>, Task<TContext>> InnerMiddleware { get; }

        public AnonymousMiddleware(IMiddleware<TContext> next, Func<TContext, IMiddleware<TContext>, Task<TContext>> innerMiddleware)
        {
            Next = next;
            InnerMiddleware = innerMiddleware;
        }

        public Task<TContext> InvokeAsync(TContext context)
        {
            return InnerMiddleware.Invoke(context, Next);
        }

    }
}
