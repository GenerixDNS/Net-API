using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.api.handler.context
{
    class SimpleChannelHandlerContext<T> : IChannelHandlerContext<T>
    {

        private T connection;
        public SimpleChannelHandlerContext(T c)
        {
            this.connection = c;    
        }

        public T Connection()
        {
            return this.connection;
        }
    }
}
