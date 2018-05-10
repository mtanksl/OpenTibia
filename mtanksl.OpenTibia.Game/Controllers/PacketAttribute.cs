using System;

namespace OpenTibia.Game
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]

    public sealed class PacketAttribute : Attribute
    {
        public PacketAttribute(byte identifier)
        {
            this.identifier = identifier;
        }

        private byte identifier;

        public byte Identifier
        {
            get
            {
                return identifier;
            }
        }
    }
}