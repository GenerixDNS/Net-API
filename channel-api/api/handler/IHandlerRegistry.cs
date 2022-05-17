using channel_api.channels.channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.handler
{
    interface IHandlerRegistry<T>
    {

        IHandlerRegistry<T> Handler(DefaultInboundHandler<T> handler);

        IHandlerRegistry<T> Unregister(int handler);

        IHandlerRegistry<T> Unregister(DefaultInboundHandler<T> handler);

        ICollection<DefaultInboundHandler<T>> Handlers();

        IChannel Channel();

    }
}
