using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;
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
            //List<Creature> canSeeFromEvents = new List<Creature>();

            Dictionary<Player, byte> canSeeFrom = new Dictionary<Player, byte>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(FromTile.Position) )
            {
                if (observer is Player player && observer != Creature)
                {
                    byte clientIndex;

                    if (player.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        canSeeFrom.Add(player, clientIndex);
                    }
                }

                //canSeeFromEvents.Add(observer);
            }

            int fromIndex = FromTile.GetIndex(Creature);

            FromTile.RemoveContent(fromIndex);

            Context.Server.Map.RemoveObserver(FromTile.Position, Creature);

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

            TileRemoveCreatureEventArgs tileRemoveCreatureEventArgs = new TileRemoveCreatureEventArgs(Creature, FromTile, fromIndex, null, null);

            //foreach (var e in canSeeFromEvents)
            //{
            //    Context.AddEvent(e, tileRemoveCreatureEventArgs);
            //}

            Context.AddEvent(tileRemoveCreatureEventArgs);

            return Promise.Completed;
        }
    }
}