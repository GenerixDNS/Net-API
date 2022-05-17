using channel_api.channels.channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.handler
{
    class SimpleHandlerRegistry<T> : IHandlerRegistry<T>
    {

        private List<DefaultInboundHandler<T>> handlers = new List<DefaultInboundHandler<T>>();

        private IChannel channel;

        public SimpleHandlerRegistry(IChannel channel)
        {
            this.channel = channel;
        }

        public IChannel Channel()
        {
            return this.channel;
        }

        public IHandlerRegistry<T> Handler(DefaultInboundHandler<T> handler)
        {
            this.handlers.Add(handler);
            return this;
        }

        public ICollection<DefaultInboundHandler<T>> Handlers()
        {
            return this.handlers;
        }

        public IHandlerRegistry<T> Unregister(int handler)
        {
            this.handlers.Remove(this.handlers[handler]);
            return this;
        }

        public IHandlerRegistry<T> Unregister(DefaultInboundHandler<T> handler)
        {
            this.handlers.Remove(handler);
            return this;
        }
    }
}
