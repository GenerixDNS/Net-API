using channel_api.api.data;
using channel_api.api.handler;
using channel_api.channels.managment;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace channel_api.channels.channels
{
    interface IChannel
    {

        IChannel Bind(IConnectionManagement m);

        IChannel Open();

        IChannel SyncUninterruptibly();

        IChannel Close();

        IHandlerRegistry<Socket> BaseRegistry();

        IChannel WriteAndFlush(string raw);

        IConnection Connection();

    }
}
