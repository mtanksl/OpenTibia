using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class TileRemoveCreatureCommand : Command
    {
        public TileRemoveCreatureCommand(Tile fromTile, Creature creature)
        {
            FromTile = fromTile;

            Creature = creature;
        }

        public Tile FromTile { get; set; }

        public Creature Creature { get; set; }

        public override Promise Execute()
        {
            Dictionary<Creature, byte> observerCanSeeFrom = new Dictionary<Creature, byte>();

            HashSet<Creature> fromCanSeeObserver = new HashSet<Creature>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(FromTile.Position) )
            {
                {
                    if (observer is Player player)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            observerCanSeeFrom.Add(player, clientIndex);
                        }
                    }
                    else
                    {
                        if (observer.Tile.Position.CanSee(FromTile.Position) )
                        {
                            observerCanSeeFrom.Add(observer, 0);
                        }
                    }
                }

                {
                    if (Creature is Player player)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(observer, out clientIndex) )
                        {
                            fromCanSeeObserver.Add(observer);
                        }
                    }
                    else
                    {
                        if (FromTile.Position.CanSee(observer.Tile.Position) )
                        {
                            fromCanSeeObserver.Add(observer);
                        }
                    }
                }
            }

            int fromIndex = FromTile.GetIndex(Creature);

            FromTile.RemoveContent(fromIndex);

            Context.Server.Map.ZoneRemoveCreature(FromTile.Position, Creature);

            foreach (var pair in observerCanSeeFrom)
            {
                if (pair.Key is Player player && player != Creature)
                {
                    if (FromTile.Count >= Constants.ObjectsPerPoint)
                    {
                        Context.AddPacket(player, new SendTileOutgoingPacket(Context.Server.Map, player.Client, FromTile.Position) );
                    }
                    else
                    {
                        Context.AddPacket(player, new ThingRemoveOutgoingPacket(FromTile.Position, pair.Value) );
                    }
                }

                Context.AddEvent(pair.Key, new CreatureDisappearEventArgs(Creature, FromTile) );
            }

            foreach (var observer in fromCanSeeObserver)
            {
                if (observer != Creature)
                {
                    Context.AddEvent(Creature, new CreatureDisappearEventArgs(observer, observer.Tile) );
                }
            }

            Context.AddEvent(new TileRemoveCreatureEventArgs(Creature, FromTile, fromIndex, null, null) );

            return Promise.Completed;
        }
    }
}