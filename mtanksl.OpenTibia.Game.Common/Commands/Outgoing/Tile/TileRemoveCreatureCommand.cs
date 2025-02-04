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
            Dictionary<Creature, byte> canSeeFrom = new Dictionary<Creature, byte>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(FromTile.Position) )
            {
                if (observer is Player player)
                {
                    if (player != Creature)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            canSeeFrom.Add(player, clientIndex);
                        }
                    }
                }
                else
                {
                    if (observer.Tile.Position.CanSee(Creature.Tile.Position) )
                    {
                        canSeeFrom.Add(observer, 0);
                    }
                }
            }

            int fromIndex = FromTile.GetIndex(Creature);

            FromTile.RemoveContent(fromIndex);

            Context.Server.Map.ZoneRemoveCreature(FromTile.Position, Creature);

            foreach (var pair in canSeeFrom)
            {
                if (pair.Key is Player player)
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

                Context.AddEvent(pair.Key, FromTile.Position, new CreatureDisappearEventArgs(Creature, FromTile, fromIndex) );
            }

            Context.AddEvent(new TileRemoveCreatureEventArgs(Creature, FromTile, fromIndex, null, null) );

            return Promise.Completed;
        }
    }
}