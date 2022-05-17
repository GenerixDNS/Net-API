using channel_api.channels.channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.data
{
    interface IConnection
    {

        IConnection PacketHandler(IPacketHandler handler);

        IChannel Channel();

        IConnection Dispatch(Packet packet);

        ICollection<IPacketHandler> Handlers();

    }
}
