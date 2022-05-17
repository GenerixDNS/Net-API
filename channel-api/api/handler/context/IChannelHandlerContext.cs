using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.api.handler.context
{
    interface IChannelHandlerContext<T>
    {

        T Connection();

    }
}
