using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                byte index = Tile.GetIndex(Creature);

                Tile.RemoveContent(index);

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer != Creature)
                    {
                        if (observer.Tile.Position.CanSee(Tile.Position) )
                        {
                            context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, index) );

                            if (Tile.Count >= Constants.ObjectsPerPoint)
                            {
                                context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(context.Server.Map, observer.Client, Tile.Position) );
                            }
                        }
                    }                
                }

                resolve(context);
            } );
        }
    }
}