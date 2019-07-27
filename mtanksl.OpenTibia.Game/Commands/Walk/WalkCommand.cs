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
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            Tile toTile = server.Map.GetTile( fromTile.Position.Offset(MoveDirection) );

            if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible),

                                                        new StopWalkOutgoingPacket(Player.Direction) );
            }
            else
            {
                Tile nextTile = GetNextTile(server, toTile);

                if (nextTile == null)
                {
                    nextTile = toTile;
                }

                SequenceCommand command = new SequenceCommand(
                            
                    new CreatureMoveCommand(Player, nextTile),
                            
                    new TurnCommand(Player, fromTile.Position.ToDirection(toTile.Position) ) );

                command.Completed += (s, e) =>
                {
                    //Act

                    base.Execute(e.Server, e.Context);
                };

                server.QueueForExecution(Constants.PlayerSchedulerEvent(Player), 1000 * fromTile.Ground.Metadata.Speed / Player.Speed, command);
            }
        }

        protected Tile GetNextTile(Server server, Tile toTile)
        {
            switch (toTile.FloorChange)
            {
                case FloorChange.Down:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, 0, 1) );

                    if (toTile != null)
                    {
                        toTile = GetNextTileDown(server, toTile);
                    }

                    break;

                case FloorChange.East:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, 0, -1) );

                    break;

                case FloorChange.North:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, -1, -1) );

                    break;

                case FloorChange.West:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, 0, -1) );

                    break;

                case FloorChange.South:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, 1, -1) );

                    break;

                case FloorChange.NorthEast:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, -1, -1) );

                    break;

                case FloorChange.NorthWest:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, -1, -1) );

                    break;

                case FloorChange.SouthEast:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, 1, -1) );

                    break;

                case FloorChange.SouthWest:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, 1, -1) );

                    break;
            }

            return toTile;
        }

        protected Tile GetNextTileDown(Server server, Tile toTile)
        {
            switch (toTile.FloorChange)
            {
                case FloorChange.East:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, 0, 0) );

                    break;

                case FloorChange.North:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, 1, 0) );

                    break;

                case FloorChange.West:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, 0, 0) );

                    break;

                case FloorChange.South:

                    toTile = server.Map.GetTile( toTile.Position.Offset(0, -1, 0) );

                    break;

                case FloorChange.NorthEast:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, 1, 0) );

                    break;

                case FloorChange.NorthWest:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, 1, 0) );

                    break;

                case FloorChange.SouthEast:

                    toTile = server.Map.GetTile( toTile.Position.Offset(-1, -1, 0) );

                    break;

                case FloorChange.SouthWest:

                    toTile = server.Map.GetTile( toTile.Position.Offset(1, -1, 0) );

                    break;
            }

            return toTile;
        }
    }
}