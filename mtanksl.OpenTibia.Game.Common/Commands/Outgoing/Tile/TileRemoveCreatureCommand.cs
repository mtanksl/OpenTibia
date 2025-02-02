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
            Dictionary<Player, byte> canSeeFrom = new Dictionary<Player, byte>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(FromTile.Position) )
            {
                if (observer != Creature)
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        canSeeFrom.Add(observer, clientIndex);
                    }
                }
            }

            int fromIndex = FromTile.GetIndex(Creature);

            FromTile.RemoveContent(fromIndex);

            Context.Server.Map.ZoneRemoveCreature(FromTile.Position, Creature);

            foreach (var pair in canSeeFrom)
            {
                if (FromTile.Count >= Constants.ObjectsPerPoint)
                {
                    Context.AddPacket(pair.Key, new SendTileOutgoingPacket(Context.Server.Map, pair.Key.Client, FromTile.Position) );
                }
                else
                {
                    Context.AddPacket(pair.Key, new ThingRemoveOutgoingPacket(FromTile.Position, pair.Value) );
                }
            }

            Context.AddEvent(new TileRemoveCreatureEventArgs(Creature, FromTile, fromIndex, null, null) );

            Context.AddEvent(new MapZoneRemoveCreatureEventArgs(Creature, FromTile, fromIndex, null, null) );

            return Promise.Completed;
        }
    }
}