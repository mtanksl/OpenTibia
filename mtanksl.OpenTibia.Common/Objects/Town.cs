using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class Town
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        public Position Position { get; set; }
    }
}