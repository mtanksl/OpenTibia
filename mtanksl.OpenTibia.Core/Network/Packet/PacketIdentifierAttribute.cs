using System;

namespace OpenTibia
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]

    public sealed class PacketIdentifierAttribute : Attribute
    {
        public PacketIdentifierAttribute(byte identifier)
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