using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
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

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                byte index = Tile.GetIndex(Creature);

                Tile.RemoveContent(index);

                if (index < Constants.ObjectsPerPoint || Tile.Count >= Constants.ObjectsPerPoint)
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        if (observer != Creature)
                        {
                            if (observer.Tile.Position.CanSee(Tile.Position) )
                            {
                                if (index < Constants.ObjectsPerPoint)
                                {
                                    Context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, index) );
                                }

                                if (Tile.Count >= Constants.ObjectsPerPoint)
                                {
                                    Context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, Tile.Position) );
                                }
                            }
                        }                
                    }
                }

                Context.AddEvent(new TileRemoveCreatureEventArgs(Tile, Creature, index) );

                resolve();
            } );
        }
    }
}