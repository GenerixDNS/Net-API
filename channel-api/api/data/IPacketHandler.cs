using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.data
{
    interface IPacketHandler
    {

        void Handle(Type type, Packet msg, JsonBuf buf);

    }
}
