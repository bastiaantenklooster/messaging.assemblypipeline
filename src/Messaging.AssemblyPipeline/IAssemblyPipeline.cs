using System;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{

    public interface IAssemblyPipeline<TContext> : IDisposable
    {

        Task<TContext> InvokeAsync(TContext context);

        AssemblyPipeline<TContext> WithMiddleware(IMiddleware<TContext> middleware);

    }

}