using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class HouseTile : Tile
    {
        public HouseTile(Position position) : base(position)
        {

        }

        public HouseTile(Position position, int capacity) : base(position, capacity)
        {

        }

        public House House { get; set; }
    }
}