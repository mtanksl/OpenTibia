using System;

namespace OpenTibia.Game
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]

    public sealed class PortAttribute : Attribute
    {
        public PortAttribute(int port)
        {
            this.port = port;
        }

        private int port;

        public int Port
        {
            get
            {
                return port;
            }
        }
    }
}