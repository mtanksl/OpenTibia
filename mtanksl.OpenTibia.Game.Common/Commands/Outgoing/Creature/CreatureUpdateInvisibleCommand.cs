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
                Dictionary<Player, byte> observerCanSeeFrom = new Dictionary<Player, byte>();

                HashSet<Creature> fromCanSeeObserver = new HashSet<Creature>();

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                {
                    {
                        byte clientIndex;

                        if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            observerCanSeeFrom.Add(observer, clientIndex);
                        }
                    }

                    {
                        if (Creature is Player player)
                        {
                            byte clientIndex;

                            if (player.Client.TryGetIndex(observer, out clientIndex) )
                            {
                                fromCanSeeObserver.Add(player);
                            }
                        }
                    }
                }

                Creature.Invisible = Invisible;

                Dictionary<Player, byte> observerCanSeeTo = new Dictionary<Player, byte>();

                HashSet<Creature> toCanSeeObserver = new HashSet<Creature>();

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                {
                    {
                        byte clientIndex;

                        if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            observerCanSeeTo.Add(observer, clientIndex);
                        }
                    }

                    {
                        if (Creature is Player player)
                        {
                            byte clientIndex;

                            if (player.Client.TryGetIndex(observer, out clientIndex) )
                            {
                                toCanSeeObserver.Add(player);
                            }
                        }
                    }
                }

                {
                    if (Creature is Player player)
                    {
                        Context.AddPacket(player, new SetOutfitOutgoingPacket(player.Id, player.Outfit) );
                    }
                }

                foreach (var observer in observerCanSeeFrom.Keys.Except(observerCanSeeTo.Keys) )
                {
                    if (observer != Creature)
                    {
                        if (Creature.Tile.Count >= Constants.ObjectsPerPoint)
                        {
                            Context.AddPacket(observer, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, Creature.Tile.Position) );
                        }
                        else
                        {
                            Context.AddPacket(observer, new ThingRemoveOutgoingPacket(Creature.Tile.Position, observerCanSeeFrom[observer] ) );
                        }

                        Context.AddEvent(observer, new CreatureDisappearEventArgs(Creature, Creature.Tile) );
                    }
                }

                foreach (var observer in fromCanSeeObserver.Except(toCanSeeObserver) )
                {
                    if (observer != Creature)
                    {
                        Context.AddEvent(Creature, new CreatureDisappearEventArgs(observer, observer.Tile) );
                    }
                }

                foreach (var observer in observerCanSeeTo.Keys.Except(observerCanSeeFrom.Keys) )
                {
                    if (observer != Creature)
                    {
                        uint removeId;

                        if (observer.Client.Battles.IsKnownCreature(Creature.Id, out removeId) )
                        {
                            Context.AddPacket(observer, new ThingAddOutgoingPacket(Creature.Tile.Position, observerCanSeeTo[observer], Creature, observer.Client.GetSkullIcon(Creature), observer.Client.GetPartyIcon(Creature) ) );
                        }
                        else
                        {
                            Context.AddPacket(observer, new ThingAddOutgoingPacket(Creature.Tile.Position, observerCanSeeTo[observer], removeId, Creature, observer.Client.GetSkullIcon(Creature), observer.Client.GetPartyIcon(Creature), observer.Client.GetWarIcon(Creature) ) );
                        }

                        Context.AddEvent(observer, new CreatureAppearEventArgs(Creature, Creature.Tile) );
                    }
                }

                foreach (var observer in toCanSeeObserver.Except(fromCanSeeObserver) )
                {
                    if (observer != Creature)
                    {
                        Context.AddEvent(Creature, new CreatureAppearEventArgs(observer, observer.Tile) );
                    }
                }

                Context.AddEvent(new CreatureUpdateInvisibleEventArgs(Creature, Invisible) );
            }

            return Promise.Completed;
        }
    }
}