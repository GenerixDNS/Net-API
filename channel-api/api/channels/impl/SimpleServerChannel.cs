using channel_api.api.exceptions;
using channel_api.api.handler;
using channel_api.api.handler.context;
using channel_api.api.pool;
using channel_api.channels.managment;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace channel_api.channels.channels.impl
{
    class SimpleServerChannel: IServerChannel
    {

        private static TcpListener locl;

        private TcpListener listener;
        private IConnectionManagement connectionManagement;
        private bool uninterruptibly = false;
        private Thread workerThread = new Thread(Listen0);
        private bool open = false;

        private static void Listen0()
        {
            while (true)
            {
                Socket socket = locl.AcceptSocket();
                ChannelPool.Pool().Register(socket);
                IChannelHandlerContext ctx = new SimpleChannelHandlerContext(socket);
                IHandlerRegistry.registries.ForEach(e =>
                {
                    foreach (DefaultHandler h in e.Handlers())
                    {
                        h.HandleClose(ctx);
                    }
                });
            }
        }

        public IChannel Bind(IConnectionManagement m)
        {
            this.connectionManagement = m;
            return this;
        }

        public IChannel Close()
        {
            if (this.open)
            {
                ChannelPool.Pool().ForEach(e =>
                {
                    IChannelHandlerContext ctx = new SimpleChannelHandlerContext(e);
                    IHandlerRegistry.registries.ForEach(e =>
                    {
                        foreach (DefaultHandler h in e.Handlers())
                        {
                            h.HandleClose(ctx);
                        }
                    });
                    e.Close();
                });
                this.listener.Stop();
                return this;
            }
            throw new ChannelNotOpenedException();
        }

        public IChannel Handler(DefaultHandler handler)
        {
            IHandlerRegistry registry = new SimpleHandlerRegistry();
            registry.Register(handler);
            return this;
        }

        public IChannel Open()
        {
            this.listener = new TcpListener(IPAddress.Any, this.connectionManagement.Port());
            locl = this.listener;
            if (this.uninterruptibly == true)
                this.workerThread.Start();
            this.listener.Start();
            return this;
        }

        public IChannel SyncUninterruptibly()
        {
            this.uninterruptibly = !this.uninterruptibly;
            return this;
        }
    }
}
