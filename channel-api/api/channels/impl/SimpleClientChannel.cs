using channel_api.api.handler;
using channel_api.channels.managment;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.channels.channels.impl
{
    class SimpleClientChannel : IChannel
    {
        private TcpClient client = new TcpClient(); 
        private IConnectionManagement connectionManagement;
        private bool uninterruptibly = false;
        private bool open = false;
        public IChannel Bind(IConnectionManagement m)
        {
            this.connectionManagement = m;
            return this;
        }

        public IChannel Close()
        {
            this.client.Close();
            return this;
        }

        public IChannel Open()
        {
            if (!this.open)
            {
                this.client.Connect(this.connectionManagement.Host(), this.connectionManagement.Port());
            } 
            return this;
        }

        public IChannel Handler(DefaultHandler handler)
        {
            IHandlerRegistry registry = new SimpleHandlerRegistry();
            registry.Register(handler);
            return this;
        }

        public IChannel SyncUninterruptibly()
        {
            this.uninterruptibly = !this.uninterruptibly;
            return this;
        }
    }
}
