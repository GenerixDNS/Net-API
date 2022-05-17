using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.channels.managment
{
    interface IConnectionManagement
    {

        public static IConnectionManagement Create(int port, string host)
        {
            SimpleConnectionManagment connectionManagment = new SimpleConnectionManagment();
            connectionManagment.Insert(host, port);
            return connectionManagment;
        }

        public static IConnectionManagement Create(int port)
        {
            SimpleConnectionManagment connectionManagment = new SimpleConnectionManagment();
            connectionManagment.Insert(port);
            return connectionManagment;
        }

        public String Host();

        public int Port();

        public void Insert(string host, int port);

        public void Insert(int port);

    }
}
