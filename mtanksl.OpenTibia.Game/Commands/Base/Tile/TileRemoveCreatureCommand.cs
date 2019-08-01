using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            byte index = Tile.GetIndex(Creature);

            Tile.RemoveContent(index);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, index) );
                }
            }

            //Event

            if (server.Events.TileRemoveCreature != null)
            {
                server.Events.TileRemoveCreature(this, new TileRemoveCreatureEventArgs(Creature, Tile, index, server, context) );
            }

            base.Execute(server, context);
        }
    }
}