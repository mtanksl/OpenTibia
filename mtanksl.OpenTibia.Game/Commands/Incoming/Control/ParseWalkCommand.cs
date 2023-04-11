using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseWalkCommand : Command
    {
        public ParseWalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }

        public override Promise Execute()
        {
            return Check(context).Then( (ctx, toTile) =>
            {
                return Promise.Delay(ctx.Server, Constants.PlayerWalkSchedulerEvent(Player), 1000 * toTile.Ground.Metadata.Speed / Player.Speed);

            } ).Then(ctx =>
			{
                return Check(ctx);

            } ).Then( (ctx, toTile) =>
            {
                return ctx.AddCommand(new CreatureUpdateParentCommand(Player, toTile) );
            } );
        }

        private PromiseResult<Tile> Check(Context context)
        {
            return Promise.Run<Tile>( (resolve, reject) =>
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
                        resolve(context, toTile);
                    }
                }
            } );
        }
    }
}