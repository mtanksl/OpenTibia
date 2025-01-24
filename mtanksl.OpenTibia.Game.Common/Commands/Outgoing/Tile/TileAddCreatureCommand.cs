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

            Context.Server.Map.AddObserver(ToTile.Position, Creature);

            Dictionary<Player, byte> canSeeTo = new Dictionary<Player, byte>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
            {
                if (observer != Creature)
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        canSeeTo.Add(observer, clientIndex);
                    }
                }
            }

            foreach (var pair in canSeeTo)
            {
                uint removeId;

                if (pair.Key.Client.Battles.IsKnownCreature(Creature.Id, out removeId) )
                {
                    Context.AddPacket(pair.Key, new ThingAddOutgoingPacket(ToTile.Position, pair.Value, Creature) );
                }
                else
                {
                    Context.AddPacket(pair.Key, new ThingAddOutgoingPacket(ToTile.Position, pair.Value, removeId, Creature) );
                }
            }

            Context.AddEvent(new TileAddCreatureEventArgs(Creature, null, null, ToTile, toIndex) );

            return Promise.Completed;
        }
    }
}