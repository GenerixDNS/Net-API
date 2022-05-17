using System;
using System.Collections.Generic;
using System.Text;

namespace channel_api.api.data.registry
{
    class DefaultPacketRegistry
    {

        private static IDictionary<Type, int> registry = new Dictionary<Type, int>();
        private static int rIndex = -1;
        public static void Register(Type type) 
        {
            rIndex = rIndex++;
            registry.Add(type, rIndex);
        }

        public static int GetUID(Type type)
        {
            return registry[type];
        }

        public static Type ByUID(int uid)
        {
            foreach(Type type in registry.Keys)
            {
                if (registry[type] == uid)
                {
                    return type;
                }
            }
            return null;
        }

    }
}
