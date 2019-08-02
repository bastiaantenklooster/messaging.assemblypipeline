using System;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline
{

    public class AssemblyPipeline<TContext> : IAssemblyPipeline<TContext>
    {

        protected IMiddleware<TContext> Outer { get; set; }

        public AssemblyPipeline()
        {
            Outer = new FinalMiddleware();
        }

        public virtual AssemblyPipeline<TContext> WithMiddleware<TMiddleware>()
            where TMiddleware : IMiddleware<TContext>
        {
            var next = Outer;
            var constructor = typeof(TMiddleware).GetConstructor(new Type[] { typeof(IMiddleware<TContext>) });

            Outer = constructor.Invoke(new object[] { next }) as IMiddleware<TContext>;

            return this;
        }

        public virtual AssemblyPipeline<TContext> WithMiddleware(IInstanceMiddleware<TContext> middleware)
        {
            var next = Outer;

            Outer = new InstanceMiddleware<TContext>(middleware, next);

            return this;
        }

        public AssemblyPipeline<TContext> WithMiddleware(Func<TContext, IMiddleware<TContext>, Task<TContext>> middleware)
        {
            var next = Outer;

            Outer = new AnonymousMiddleware<TContext>(next, middleware);

            return this;
        }

        public AssemblyPipeline<TContext> WithMiddleware(Func<TContext, IMiddleware<TContext>, TContext> middleware)
        {
            return WithMiddleware((context, next) => Task.FromResult(middleware.Invoke(context, next)));
        }

        public virtual Task<TContext> InvokeAsync(TContext context)
        {
            if (Outer == null)
                return Task.FromResult(context);

            return Outer.InvokeAsync(context);
        }

        protected class FinalMiddleware : IMiddleware<TContext>
        {

            public Task<TContext> InvokeAsync(TContext context)
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
