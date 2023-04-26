using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class TileRemoveCreatureCommand : Command
    {
        public TileRemoveCreatureCommand(Tile tile, Creature creature)
        {
            Tile = tile;

            Creature = creature;
        }

        public Tile Tile { get; set; }

        public Creature Creature { get; set; }

        public override Promise Execute()
        {
            var canSeeFrom = new Dictionary<Player, byte>();

            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
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

            byte index = Tile.GetIndex(Creature);

            Tile.RemoveContent(index);

            foreach (var pair in canSeeFrom)
            {
                if (Tile.Count >= Constants.ObjectsPerPoint)
                {
                    Context.AddPacket(pair.Key.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, pair.Key.Client, Tile.Position) );
                }
                else
                {
                    Context.AddPacket(pair.Key.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, pair.Value) );
                }
            }

            Context.AddEvent(new TileRemoveCreatureEventArgs(Tile, Creature, index) );

            return Promise.Completed;
        }
    }
}