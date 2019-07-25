using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileAddCreatureCommand : Command
    {
        public TileAddCreatureCommand(Tile tile, Creature creature)
        {
            Tile = tile;

            Creature = creature;
        }

        public Tile Tile { get; set; }

        public Creature Creature { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Act

            server.Map.AddCreature(Creature);

            byte toIndex = Tile.AddContent(Creature);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if ( observer.Tile.Position.CanSee(Tile.Position) )
                {
                    uint removeId;

                    if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                    {
                        context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, toIndex, Creature) );
                    }
                    else
                    {
                        context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, toIndex, removeId, Creature) );
                    }
                }
            }

            base.Execute(server, context);
        }
    }
}