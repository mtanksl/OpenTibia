using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileAddCreatureCommand : Command
    {
        public TileAddCreatureCommand(Tile tile, Creature creature)
        {
            Tile = tile;

            Creature = creature;
        }

        public Tile Tile { get; set; }

        public Creature Creature { get; set; }

        public override void Execute(Context context)
        {
            byte index = Tile.AddContent(Creature);

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (observer != Creature)
                {
                    if (observer.Tile.Position.CanSee(Tile.Position) )
                    {
                        uint removeId;

                        if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                        {
                            context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, Creature) );
                        }
                        else
                        {
                            context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, removeId, Creature) );
                        }
                    }
                }
            }

            OnComplete(context);
        }
    }
}