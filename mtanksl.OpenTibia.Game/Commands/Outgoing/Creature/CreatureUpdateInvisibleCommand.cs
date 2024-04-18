using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;
using System.Linq;

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
                Dictionary<Player, byte> canSeeFrom = new Dictionary<Player, byte>();

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        canSeeFrom.Add(observer, clientIndex);
                    }
                }

                Creature.Invisible = Invisible;

                Dictionary<Player, byte> canSeeTo = new Dictionary<Player, byte>();

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        canSeeTo.Add(observer, clientIndex);
                    }
                }

                foreach (var observer in canSeeFrom.Keys.Except(canSeeTo.Keys) )
                {
                    if (observer != Creature)
                    {
                        if (Creature.Tile.Count >= Constants.ObjectsPerPoint)
                        {
                            Context.AddPacket(observer, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, Creature.Tile.Position) );
                        }
                        else
                        {
                            Context.AddPacket(observer, new ThingRemoveOutgoingPacket(Creature.Tile.Position, canSeeFrom[observer] ) );
                        }
                    }
                }

                foreach (var observer in canSeeTo.Keys.Except(canSeeFrom.Keys) )
                {
                    if (observer != Creature)
                    {
                        uint removeId;

                        if (observer.Client.Battles.IsKnownCreature(Creature.Id, out removeId) )
                        {
                            Context.AddPacket(observer, new ThingAddOutgoingPacket(Creature.Tile.Position, canSeeTo[observer], Creature) );
                        }
                        else
                        {
                            Context.AddPacket(observer, new ThingAddOutgoingPacket(Creature.Tile.Position, canSeeTo[observer], removeId, Creature) );
                        }
                    }
                }

                Context.AddEvent(new CreatureUpdateInvisibleEventArgs(Creature, Invisible) );
            }

            return Promise.Completed;
        }
    }
}