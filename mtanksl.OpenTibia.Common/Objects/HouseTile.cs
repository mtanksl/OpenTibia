using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class HouseTile : Tile
    {
        public HouseTile(Position position) : base(position)
        {

        }

        public House House { get; set; }
    }
}