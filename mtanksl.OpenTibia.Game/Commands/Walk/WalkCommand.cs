using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class WalkCommand : Command
    {
        public WalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }


        private int index = 0;

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            Tile toTile = server.Map.GetTile( fromTile.Position.Offset(MoveDirection) );

            //Act

            if ( toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible), 
                    
                                                        new StopWalkOutgoingPacket(Player.Direction) );
            }
            else
            {
                if (index++ == 0)
                {
                    server.QueueForExecution(Constants.PlayerSchedulerEvent(Player), 1000 * fromTile.Ground.Metadata.Speed / Player.Speed, this);
                }
                else
                {
                    new CreatureMoveCommand(Player, toTile).Execute(server, context);
           
                    base.Execute(server, context);
                }
            }
        }
    }
}