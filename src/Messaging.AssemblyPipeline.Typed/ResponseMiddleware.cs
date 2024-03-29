﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.AssemblyPipeline.Typed
{
    class ResponseMiddleware : IMiddleware<Context>
    {

        private Dictionary<Type, Type> AllowedResponses { get; } = new Dictionary<Type, Type>();

        public void AddResponseType<TRequest, TResponse>()
        {
            AllowedResponses.TryAdd(typeof(TRequest), typeof(TResponse));
        }

        public Task<Context> InvokeAsync(Context context, MiddlewareDelegate<Context> next)
        {
            if (context.Response == null)
                return next(context);

            var requestType = context.Request.GetType();
            var responseTypeAllowed = AllowedResponses.GetValueOrDefault(requestType);

            if (responseTypeAllowed != null && !responseTypeAllowed.IsInstanceOfType(context.Response))
                throw new InvalidOperationException("Invalid response type.");

            return next(context);
        }

    }
}
