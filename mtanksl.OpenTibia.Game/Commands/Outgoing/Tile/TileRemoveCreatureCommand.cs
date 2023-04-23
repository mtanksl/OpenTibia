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
            var updates = new Queue< (Player Player, byte ClientIndex) >();

            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                if (observer != Creature)
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        updates.Enqueue( (observer, clientIndex) );
                    }
                }
            }

            byte index = Tile.GetIndex(Creature);

            Tile.RemoveContent(index);

            while (updates.Count > 0)
            {
                var update = updates.Dequeue();

                if (Tile.Count >= Constants.ObjectsPerPoint)
                {
                    Context.AddPacket(update.Player.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, update.Player.Client, Tile.Position) );
                }
                else
                {
                    Context.AddPacket(update.Player.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, update.ClientIndex) );
                }
            }

            Context.AddEvent(new TileRemoveCreatureEventArgs(Tile, Creature, index) );

            return Promise.Completed;
        }
    }
}