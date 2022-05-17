using channel_api.api.handler;
using channel_api.channels.managment;
using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.channels.channels
{
    interface IChannel
    {

        IChannel Bind(IConnectionManagement m);

        IChannel Open();

        IChannel SyncUninterruptibly();

        IChannel Close();

        IChannel Handler(DefaultHandler handler);

    }
}
