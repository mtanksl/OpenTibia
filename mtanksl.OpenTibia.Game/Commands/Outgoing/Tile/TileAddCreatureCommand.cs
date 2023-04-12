using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
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

        public override Promise Execute()
        {
            byte index = Tile.AddContent(Creature);

            if (index < Constants.ObjectsPerPoint)
            {
                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (observer != Creature)
                    {
                        if (observer.Tile.Position.CanSee(Tile.Position) )
                        {
                            uint removeId;

                            if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                            {
                                Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, Creature) );
                            }
                            else
                            {
                                Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, removeId, Creature) );
                            }
                        }
                    }
                }
            }

            Context.AddEvent(new TileAddCreatureEventArgs(Tile, Creature, index) );

            return Promise.Completed;
        }
    }
}