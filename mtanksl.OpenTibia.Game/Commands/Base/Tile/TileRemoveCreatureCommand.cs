using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileRemoveCreatureCommand : Command
    {
        public TileRemoveCreatureCommand(Tile tile, Creature creature)
        {
            Tile = tile;

            Creature = creature;
        }

        public Tile Tile { get; set; }

        public Creature Creature { get; set; }

        public override void Execute(Context context)
        {
            byte index = Tile.GetIndex(Creature);

            Tile.RemoveContent(index);

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, index) );
                }
            }

            //Event

            if (context.Server.Events.TileRemoveCreature != null)
            {
                context.Server.Events.TileRemoveCreature(this, new TileRemoveCreatureEventArgs(Tile, Creature, index) );
            }

            base.OnCompleted(context);
        }
    }
}