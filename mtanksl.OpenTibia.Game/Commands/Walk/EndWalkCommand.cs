using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class EndWalkCommand : Command
    {
        public EndWalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }

        public override void Execute(Context context)
        {
            Tile fromTile = Player.Tile;

            if (fromTile != null)
            {
                Tile toTile = context.Server.Map.GetTile(fromTile.Position.Offset(MoveDirection) );

                if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
                {
                    context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible), 
                    
                                                                  new StopWalkOutgoingPacket(Player.Direction) );
                }
                else
                {
                    Command command = context.TransformCommand(new CreatureMoveCommand(Player, toTile) );

                    command.Completed += (s, e) =>
                    {
                        base.OnCompleted(e.Context);
                    };

                    command.Execute(context);
                }
            }
        }
    }
}