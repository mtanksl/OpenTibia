using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class ChangeOutfitCommand : Command
    {
        private Server server;

        public ChangeOutfitCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Outfit ToOutfit { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            Outfit fromOutfit = Player.Outfit;

            //Act

            Player.Outfit = ToOutfit;

            AddEvent(new ChangedOutfitEventArgs(server)
            {
                Player = Player,

                FromTile = fromTile,

                FromIndex = fromIndex,

                FromOutfit = fromOutfit,

                ToOutfit = ToOutfit
            } );
        }
    }
}