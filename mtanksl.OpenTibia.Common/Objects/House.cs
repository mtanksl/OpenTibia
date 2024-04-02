using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class House
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        public Position Entry { get; set; }

        public Town Town { get; set; }

        public uint Rent { get; set; }

        public uint Size { get; set; }

        public bool Guildhall { get; set; }
    }
}