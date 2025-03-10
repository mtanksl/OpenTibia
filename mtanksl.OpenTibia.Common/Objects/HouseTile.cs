using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class HouseTile : Tile
    {
        public HouseTile(Position position) : base(position)
        {

        }

        public HouseTile(Position position, int internalListCapacity) : base(position, internalListCapacity)
        {

        }

        public House House { get; set; }
    }
}