using channel_api.api.handler.context;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.api.handler
{
    interface DefaultInboundHandler<T>
    {

        void HandleClose(IChannelHandlerContext<T> ctx);

        void HandleOpen(IChannelHandlerContext<T> ctx);

        void ChannelRead0(IChannelHandlerContext<T> ctx, string msg);

    }
}
