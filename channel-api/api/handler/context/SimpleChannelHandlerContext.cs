using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.api.handler.context
{
    class SimpleChannelHandlerContext : IChannelHandlerContext
    {

        private Socket connection;
        SimpleChannelHandlerContext(Socket c)
        {
            this.connection = c;    
        }

        public Socket Connection()
        {
            return this.connection;
        }
    }
}
