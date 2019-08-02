using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.AssemblyPipeline.Typed
{
    public class Context
    {

        public Interface1 Request { get; }

        public Interface1 Response { get; set; }

        public Context(Interface1 request)
        {
            Request = request;
        }

    }
}
