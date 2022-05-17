using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.handler
{
    interface IHandlerRegistry
    {

        IHandlerRegistry Register(DefaultHandler handler);

        IHandlerRegistry Unregister(int i);

        IHandlerRegistry Unregister(DefaultHandler handler);

        ICollection<DefaultHandler> Handlers();

    }
}
