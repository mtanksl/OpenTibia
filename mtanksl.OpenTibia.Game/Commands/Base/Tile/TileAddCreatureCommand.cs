using OpenTibia.Common.Events;
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

        public override void Execute(Context context)
        {
            //Arrange

            //Act

            byte index = Tile.AddContent(Creature);

            //Notify

            foreach (var observer in context.Server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    uint removeId;

                    if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                    {
                        context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, Creature) );
                    }
                    else
                    {
                        context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, removeId, Creature) );
                    }
                }
            }

            //Event

            if (context.Server.Events.TileAddCreature != null)
            {
                context.Server.Events.TileAddCreature(this, new TileAddCreatureEventArgs(Tile, Creature, index) );
            }

            base.Execute(context);
        }
    }
}