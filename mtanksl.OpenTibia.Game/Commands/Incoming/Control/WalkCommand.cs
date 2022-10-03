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

        public override void Execute(Context context)
        {
            Tile fromTile = Player.Tile;

            if (fromTile != null)
            {
                Tile toTile = context.Server.Map.GetTile(fromTile.Position.Offset(MoveDirection) );

                if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
                {
                    context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible),

                                                                new StopWalkOutgoingPacket(Player.Direction) );
                }
                else
                {
                    Promise.Delay(context, Constants.CreatureWalkSchedulerEvent(Player), 1000 * toTile.Ground.Metadata.Speed / Player.Speed).Then(ctx =>
                    {
                        EndWalk(ctx);
                    } );
                }
            }
        }

        private void EndWalk(Context context)
        {
            Tile fromTile = Player.Tile;

            if (fromTile != null)
            {
                Tile toTile = context.Server.Map.GetTile(fromTile.Position.Offset(MoveDirection) );

                if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
                {
                    context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible),

                                                                new StopWalkOutgoingPacket(Player.Direction) );
                }
                else
                {
                    context.AddCommand(new CreatureUpdateParentCommand(Player, toTile) ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );
                }
            }
        }
    }
}