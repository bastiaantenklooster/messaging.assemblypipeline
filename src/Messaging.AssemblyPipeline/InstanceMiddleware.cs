using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{
    internal class InstanceMiddleware<TContext> : IMiddleware<TContext>
    {

        protected IInstanceMiddleware<TContext> InnerMiddleware { get; }

        protected IMiddleware<TContext> Next { get; }

        public InstanceMiddleware(IInstanceMiddleware<TContext> innerMiddleware, IMiddleware<TContext> next)
        {
            InnerMiddleware = innerMiddleware;
            Next = next;
        }

        public Task<TContext> InvokeAsync(TContext context)
        {
            return InnerMiddleware.InvokeAsync(context, Next);
        }

    }
}
