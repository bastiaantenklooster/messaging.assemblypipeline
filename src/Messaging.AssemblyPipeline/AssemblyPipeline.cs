using System;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{

    public delegate Task<TContext> MiddlewareDelegate<TContext>(TContext context);

    public class AssemblyPipeline<TContext> : IAssemblyPipeline<TContext>
    {

        protected MiddlewareDelegate<TContext> Outer { get; set; }

        public AssemblyPipeline()
        {
            Outer = context => new FinalMiddleware().InvokeAsync(context, null);
        }

        public virtual AssemblyPipeline<TContext> WithMiddleware<TMiddleware>()
            where TMiddleware : IMiddleware<TContext>, new()
        {
            return WithMiddleware(new TMiddleware());
        }

        public virtual AssemblyPipeline<TContext> WithMiddleware(IMiddleware<TContext> middleware)
        {
            if (middleware == null)
                throw new ArgumentNullException(nameof(middleware));

            return WithMiddleware(middleware.InvokeAsync);
        }

        public virtual AssemblyPipeline<TContext> WithMiddleware(Func<TContext, MiddlewareDelegate<TContext>, Task<TContext>> middleware)
        {
            if (middleware == null)
                throw new ArgumentNullException(nameof(middleware));

            var next = Outer;

            Outer = new MiddlewareDelegate<TContext>(context => middleware.Invoke(context, next));

            return this;
        }

        public virtual Task<TContext> InvokeAsync(TContext context)
        {
            if (Outer == null)
                return Task.FromResult(context);

            return Outer.Invoke(context);
        }

        protected class FinalMiddleware : IMiddleware<TContext>
        {
            public Task<TContext> InvokeAsync(TContext context, MiddlewareDelegate<TContext> next)
            {
                return Task.FromResult(context);
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Outer = null;

                disposedValue = true;
            }
        }

        ~AssemblyPipeline()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
