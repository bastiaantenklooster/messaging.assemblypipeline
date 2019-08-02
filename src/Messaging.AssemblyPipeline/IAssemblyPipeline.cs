using System;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{

    public interface IAssemblyPipeline<TContext> : IDisposable
    {

        Task<TContext> InvokeAsync(TContext context);

        AssemblyPipeline<TContext> WithMiddleware<TMiddleware>() where TMiddleware : IMiddleware<TContext>;

        AssemblyPipeline<TContext> WithMiddleware(IInstanceMiddleware<TContext> middleware);

        AssemblyPipeline<TContext> WithMiddleware(Func<TContext, IMiddleware<TContext>, Task<TContext>> middleware);

        AssemblyPipeline<TContext> WithMiddleware(Func<TContext, IMiddleware<TContext>, TContext> middleware);

    }

}