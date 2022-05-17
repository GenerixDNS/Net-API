using channel_api.api.handler.context;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.api.handler
{
    interface DefaultHandler
    {

        void HandleClose(IChannelHandlerContext ctx);

        void HandleOpen(IChannelHandlerContext ctx);

    }
}
