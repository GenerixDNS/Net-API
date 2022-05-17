using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.handler
{
    class SimpleHandlerRegistry : IHandlerRegistry
    {
        public SimpleHandlerRegistry()
        {
            IHandlerRegistry.registries.Add(this);
        }

        private List<DefaultHandler> handlers = new List<DefaultHandler>();

        public ICollection<DefaultHandler> Handlers()
        {
            return this.handlers;
        }

        public IHandlerRegistry Register(DefaultHandler handler)
        {
            this.handlers.Add(handler);
            return this;
        }

        public IHandlerRegistry Unregister(int i)
        {
            this.handlers.Remove(this.handlers[i]);
            return this;
        }

        public IHandlerRegistry Unregister(DefaultHandler handler)
        {
            this.handlers.Remove(handler);
            return this;
        }
    }
}
