using channel_api.api.handler;
using channel_api.channels.channels.impl;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.channels.channels
{
    interface IConnectedChannel: IChannel
    {

        public static IChannel getChannel()
        {
            return new SimpleClientChannel();
        }

    }
}
