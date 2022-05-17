using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.channels.managment
{
    class SimpleConnectionManagment : IConnectionManagement
    {

        private int port;
        private string host;

        public string Host()
        {
            return this.host;
        }

        public void Insert(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Insert(int port)
        {
            this.port = port;
        }

        public int Port()
        {
            return this.port;
        }
    }
}
