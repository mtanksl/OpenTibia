using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveCreatureCommand : Command
    {
        public PlayerMoveCreatureCommand(Player player, Creature item, Tile toTile)
        {
            Player = player;

            Creature = item;

            ToTile = toTile;
        }

        public Player Player { get; set; }

        public Creature Creature { get; set; }

        public Tile ToTile { get; set; }

        protected bool IsEnoughtRoom(Context context, Tile fromTile, Tile toTile)
        {
            if (toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtRoom) );

                return false;
            }

            return true;
        }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (IsEnoughtRoom(context, Creature.Tile, ToTile) )
                {
                    context.AddCommand(new CreatureUpdateParentCommand(Creature, ToTile) ).Then(ctx =>
                    {
                        resolve(ctx);
                    } );
                }
            } );
        }
    }
}