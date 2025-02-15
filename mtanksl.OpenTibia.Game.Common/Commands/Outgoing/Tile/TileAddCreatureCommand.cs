using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class TileAddCreatureCommand : Command
    {
        public TileAddCreatureCommand(Tile toTile, Creature creature)
        {
            ToTile = toTile;

            Creature = creature;
        }

        public Tile ToTile { get; set; }

        public Creature Creature { get; set; }

        public override Promise Execute()
        {
            int toIndex = ToTile.AddContent(Creature);

            Context.Server.Map.ZoneAddCreature(ToTile.Position, Creature);

            Dictionary<Creature, byte> observerCanSeeTo = new Dictionary<Creature, byte>();

            HashSet<Creature> toCanSeeObserver = new HashSet<Creature>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(ToTile.Position) )
            {
                {
                    if (observer is Player player)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            observerCanSeeTo.Add(player, clientIndex);
                        }
                    }
                    else
                    {
                        if (observer.Tile.Position.CanSee(ToTile.Position) )
                        {
                            observerCanSeeTo.Add(observer, 0);
                        }
                    }
                }

                {
                    if (Creature is Player player)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(observer, out clientIndex) )
                        {
                            toCanSeeObserver.Add(observer);
                        }
                    }
                    else
                    {
                        if (ToTile.Position.CanSee(observer.Tile.Position) )
                        {
                            toCanSeeObserver.Add(observer);
                        }
                    }
                }
            }

            foreach (var pair in observerCanSeeTo)
            {
                if (pair.Key is Player player && player != Creature)
                {
                    uint removeId;

                    if (player.Client.Battles.IsKnownCreature(Creature.Id, out removeId) )
                    {
                        Context.AddPacket(player, new ThingAddOutgoingPacket(ToTile.Position, pair.Value, Creature, player.Client.GetClientSkullIcon(Creature), player.Client.GetClientPartyIcon(Creature)));
                    }
                    else
                    {
                        Context.AddPacket(player, new ThingAddOutgoingPacket(ToTile.Position, pair.Value, removeId, Creature, player.Client.GetClientSkullIcon(Creature), player.Client.GetClientPartyIcon(Creature), player.Client.GetClientWarIcon(Creature)));
                    }
                }

                Context.AddEvent(pair.Key, new CreatureAppearEventArgs(Creature, ToTile) );
            }

            foreach (var observer in toCanSeeObserver)
            {
                if (observer != Creature)
                {
                    Context.AddEvent(Creature, new CreatureAppearEventArgs(observer, observer.Tile) );
                }
            }

            Context.AddEvent(new TileAddCreatureEventArgs(Creature, null, null, ToTile, toIndex) );

            return Promise.Completed;
        }
    }
}