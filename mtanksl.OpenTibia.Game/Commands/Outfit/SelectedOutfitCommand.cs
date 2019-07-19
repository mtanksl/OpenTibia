using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class SelectedOutfitCommand : Command
    {
        public SelectedOutfitCommand(Player player, Outfit outfit)
        {
            Player = player;

            Outfit = outfit;
        }

        public Player Player { get; set; }

        public Outfit Outfit { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            //Act

            Player.Outfit = Outfit;

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(fromTile.Position) )
                {
                    context.Write(observer.Client.Connection, new SetOutfitOutgoingPacket(Player.Id, Outfit) );
                }
            }
        }
    }
}