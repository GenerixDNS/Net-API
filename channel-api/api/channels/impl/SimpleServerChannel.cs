using channel_api.api.data;
using channel_api.api.data.registry;
using channel_api.api.exceptions;
using channel_api.api.handler;
using channel_api.api.handler.context;
using channel_api.api.pool;
using channel_api.channels.managment;
using Newtonsoft.Json;
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
        private static Encoding encoder = Encoding.UTF8;
        private static IConnection connection;

        private TcpListener listener;
        private IConnectionManagement connectionManagement;
        private bool uninterruptibly = false;
        private Thread workerThread = new Thread(Listen0);
        private Thread channelReadThread = new Thread(ListenRead0);
        private bool open = false;
        private IHandlerRegistry<Socket> registry;

        public SimpleServerChannel()
        {
            this.registry = new SimpleHandlerRegistry<Socket>(this);
            connection = new SimpleConnection(this);
        }

        private static void Listen0()
        {
            while (true)
            {
                Socket socket = locl.AcceptSocket();
                ChannelPool.Pool().Register(socket);
                IChannelHandlerContext<Socket> ctx = new SimpleChannelHandlerContext<Socket>(socket);
                ChannelPool.Pool().ForEachRegistry(e =>
                {
                    foreach (DefaultInboundHandler<Socket> h in e.Handlers())
                    {
                        h.HandleOpen(ctx);
                    }
                });
            }
        }

        private static void ListenRead0()
        {
            while (true)
            {
                if (locl.Server.Available > 0)
                {
                    string receivedValue = string.Empty;
                    byte[] receivedBytes = new byte[locl.Server.ReceiveBufferSize];
                    int numBytes = locl.Server.Receive(receivedBytes);
                    receivedValue += Encoding.UTF8.GetString(receivedBytes);
                    bool p = false;
                    try
                    {
                        JsonBuf buf = JsonConvert.DeserializeObject<JsonBuf>(receivedValue);
                        Packet packet = (Packet)buf.Raw();
                        Type type = DefaultPacketRegistry.ByUID(buf.Uid());
                        foreach (IPacketHandler handler in connection.Handlers())
                            handler.Handle(type, packet, buf);
                        p = true;
                    }
                    catch { }
                    if (!p)
                    {
                        ChannelPool.Pool().ForEachRegistry(e =>
                        {
                            foreach (DefaultInboundHandler<Socket> h in e.Handlers())
                            {
                                h.ChannelRead0(null, receivedValue);
                            }
                        });
                    }
                }
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
                    IChannelHandlerContext<Socket> ctx = new SimpleChannelHandlerContext<Socket>(e);
                    ChannelPool.Pool().ForEachRegistry(e =>
                    {
                        foreach (DefaultInboundHandler<Socket> h in e.Handlers())
                        {
                            h.HandleClose(ctx);
                        }
                    });
                    e.Close();
                });
                this.listener.Stop();
                this.workerThread.Abort();
                this.channelReadThread.Abort();
                return this;
            }
            throw new ChannelNotOpenedException();
        }

        public IChannel Open()
        {
            this.listener = new TcpListener(IPAddress.Any, this.connectionManagement.Port());
            locl = this.listener;
            this.listener.Start();
            this.channelReadThread.Start();
            if (this.uninterruptibly == true)
                this.workerThread.Start();
            else Listen0();
            return this;
        }

        public IChannel SyncUninterruptibly()
        {
            this.uninterruptibly = !this.uninterruptibly;
            return this;
        }

        public IChannel WriteAndFlush(string raw)
        {
            byte[] bytes = encoder.GetBytes(raw);
            locl.Server.Send(bytes);
            return this;
        }

        public IHandlerRegistry<Socket> BaseRegistry()
        {
            return this.registry;
        }

        public IConnection Connection()
        {
            return connection;
        }
    }
}
