using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{

    public interface IMiddleware<TContext>
    {

        Task<TContext> InvokeAsync(TContext context);

    }

}
