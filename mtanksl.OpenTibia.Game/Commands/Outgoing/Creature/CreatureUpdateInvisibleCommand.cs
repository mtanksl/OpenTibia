using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateInvisibleCommand : Command
    {
        public CreatureUpdateInvisibleCommand(Creature creature, bool invisible)
        {
            Creature = creature;

            Invisible = invisible;
        }

        public Creature Creature { get; set; }

        public bool Invisible { get; set; }

        public override Promise Execute()
        {
            if (Creature.Invisible != Invisible)
            {
                if (Invisible)
                {
                    foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                    {
                        if (observer != Creature)
                        {
                            byte clientIndex;

                            if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                            {
                                if (Creature.Tile.Count - 1 >= Constants.ObjectsPerPoint)
                                {
                                    Context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, Creature.Tile.Position) );
                                }
                                else
                                {
                                    Context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(Creature.Tile.Position, clientIndex) );
                                }
                            }
                        }
                    }

                    Creature.Invisible = Invisible;
                }
                else
                {
                    Creature.Invisible = Invisible;

                    foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                    {
                        if (observer != Creature)
                        {
                            byte clientIndex;

                            if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                            {
                                uint removeId;

                                if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                                {
                                    Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Creature.Tile.Position, clientIndex, Creature) );
                                }
                                else
                                {
                                    Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Creature.Tile.Position, clientIndex, removeId, Creature) );
                                }
                            }
                        }
                    }
                }

                Context.AddEvent(new CreatureUpdateInvisibleEventArgs(Creature, Invisible) );
            }

            return Promise.Completed;
        }
    }
}