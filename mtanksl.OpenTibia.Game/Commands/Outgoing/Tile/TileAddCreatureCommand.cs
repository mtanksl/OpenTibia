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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                byte index = Tile.AddContent(Creature);

                if (index < Constants.ObjectsPerPoint)
                {
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
                }

                context.AddEvent(new TileAddCreatureEventArgs(Tile, Creature, index) );

                resolve(context);
            } );
        }
    }
}