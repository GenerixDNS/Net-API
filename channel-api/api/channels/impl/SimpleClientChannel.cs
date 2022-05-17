using channel_api.api.data;
using channel_api.api.data.registry;
using channel_api.api.handler;
using channel_api.api.handler.context;
using channel_api.api.pool;
using channel_api.channels.managment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace channel_api.channels.channels.impl
{
    class SimpleClientChannel : IConnectedChannel
    {

        private static Encoding encoder = Encoding.UTF8;
        private static Socket locl;
        private static IChannelHandlerContext<Socket> ctx;
        private static IConnection connection;

        private TcpClient client = new TcpClient();
        private IConnectionManagement connectionManagement;
        private bool uninterruptibly = false;
        private bool open = false;
        private Thread channelReadThread = new Thread(ListenRead0);
        private IHandlerRegistry<Socket> registry;

        public SimpleClientChannel()
        {
            this.registry = new SimpleHandlerRegistry<Socket>(this);
            connection = new SimpleConnection(this);
        }
        public static void ListenRead0()
        {
            while (true)
            {
                if (locl.Available > 0)
                {
                    string receivedValue = string.Empty;
                    byte[] receivedBytes = new byte[locl.ReceiveBufferSize];
                    int numBytes = locl.Receive(receivedBytes);
                    receivedValue += Encoding.UTF8.GetString(receivedBytes);
                    bool p = false;
                    try
                    {
                        JsonBuf buf = JsonConvert.DeserializeObject<JsonBuf>(receivedValue);
                        Packet packet = (Packet)buf.Raw();
                        Type type = DefaultPacketRegistry.ByUID(buf.Uid());
                        foreach(IPacketHandler handler in connection.Handlers())
                            handler.Handle(type, packet, buf);
                        p = true;
                    } catch{}
                    if (!p)
                    {
                        ChannelPool.Pool().ForEachRegistry(e =>
                        {
                            foreach (DefaultInboundHandler<Socket> h in e.Handlers())
                            {
                                h.ChannelRead0(ctx, receivedValue);
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
            ChannelPool.Pool().ForEachRegistry(e =>
            {
                foreach (DefaultInboundHandler<Socket> h in e.Handlers())
                {
                    h.HandleClose(ctx);
                }
            });
            this.channelReadThread.Abort();
            this.client.Close();
            return this;
        }

        public IChannel Open()
        {
            if (!this.open)
            {
                this.client.Connect(this.connectionManagement.Host(), this.connectionManagement.Port());
                locl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                channelReadThread.Start();
                ctx = new SimpleChannelHandlerContext<Socket>(locl);
                ChannelPool.Pool().ForEachRegistry(e =>
                {
                    foreach (DefaultInboundHandler<Socket> h in e.Handlers())
                    {
                        h.HandleOpen(ctx);
                    }
                });
            } 
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
            locl.Send(bytes);
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
