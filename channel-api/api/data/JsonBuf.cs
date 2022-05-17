using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.data
{
    class JsonBuf
    {

        private object raw;
        private int uid;

        public JsonBuf(object t, int i)
        {
            this.raw = t;
            this.uid = i;
        }

        public int Uid()
        {
            return this.uid;
        }

        public object Raw()
        {
            return this.raw;
        }

    }
}
