using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.api.pool
{
    class ChannelPool
    {

        private static ChannelPool instance;
        private List<Socket> registry = new List<Socket>();

        public static ChannelPool Pool() 
        {
            return instance;
        }

        ChannelPool()
        {
            instance = this;
        }
        
        public void Register(Socket socket)
        {
            this.registry.Add(socket);
        }

        public void ForEach(Action<Socket> a)
        {
            foreach (Socket socket in this.registry)
            {
                socket.Close();
            }
        }

    }
}
