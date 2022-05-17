using channel_api.api.data.registry;
using channel_api.channels.channels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.data
{
    class SimpleConnection : IConnection
    {

        private IChannel channel;
        private List<IPacketHandler> handlers = new List<IPacketHandler>();

        public SimpleConnection(IChannel channel)
        {
            this.channel = channel;
        }
        public IChannel Channel()
        {
            return this.channel;
        }

        public IConnection Dispatch(Packet packet)
        {
            JsonBuf buf = new JsonBuf(packet, DefaultPacketRegistry.GetUID(packet.GetType()));
            this.channel.WriteAndFlush(JsonConvert.SerializeObject(buf));
            return this;
        }

        public IConnection PacketHandler(IPacketHandler handler)
        {
            this.handlers.Add(handler);
            return this;
        }

        ICollection<IPacketHandler> IConnection.Handlers()
        {
            return this.handlers;
        }

    }
}
