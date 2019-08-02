using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{
    public interface IInstanceMiddleware<TContext>
    {

        Task<TContext> InvokeAsync(TContext context, IMiddleware<TContext> next);

    }
}
