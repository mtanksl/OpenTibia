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
            return Check().Then( (toTile) =>
            {
                return Promise.Delay(Context.Server, Constants.PlayerWalkSchedulerEvent(Player), 1000 * toTile.Ground.Metadata.Speed / Player.Speed);

            } ).Then( () =>
			{
                return Check();

            } ).Then( (toTile) =>
            {
                return Context.AddCommand(new CreatureUpdateParentCommand(Player, toTile) );
            } );
        }

        private PromiseResult<Tile> Check()
        {
            return Promise.Run<Tile>( (resolve, reject) =>
            {
                Tile fromTile = Player.Tile;

                if (fromTile != null)
                {
                    Tile toTile = Context.Server.Map.GetTile(fromTile.Position.Offset(MoveDirection) );

                    if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
                    {
                        Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible),

                                                                    new StopWalkOutgoingPacket(Player.Direction) );
                    }
                    else
                    {
                        resolve(toTile);
                    }
                }
            } );
        }
    }
}