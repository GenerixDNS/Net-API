using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.handler
{
    interface IHandlerRegistry
    {

        public static List<IHandlerRegistry> registries = new List<IHandlerRegistry>();

        IHandlerRegistry Register(DefaultHandler handler);

        IHandlerRegistry Unregister(int i);

        IHandlerRegistry Unregister(DefaultHandler handler);

        ICollection<DefaultHandler> Handlers();

    }
}
