using channel_api.api.handler;
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
        private List<IHandlerRegistry<object>> registries = new List<IHandlerRegistry<object>>();

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
                a.Invoke(socket);
            }
        }

        public void ForEachRegistry(Action<IHandlerRegistry<object>> a)
        {
            foreach (IHandlerRegistry<object> registry in this.registries)
            {
                a.Invoke(registry);
            }
        }

    }
}
