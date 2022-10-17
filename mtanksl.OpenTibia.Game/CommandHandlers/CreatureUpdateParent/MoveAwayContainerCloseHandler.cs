using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveAwayContainerCloseHandler : CommandHandler<CreatureUpdateParentCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, CreatureUpdateParentCommand command)
        {
            if (command.Creature is Player player && player.Client.ContainerCollection.GetContainers().Any(c => c.Root() is Tile || c.Metadata.OpenTibiaId == 2594) )
            {
                return next(context).Then(ctx =>
                {
                    foreach (var pair in player.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        if (pair.Value.Root() is Tile tile)
                        {
                            if ( !command.ToTile.Position.IsNextTo(tile.Position) )
                            {
                                player.Client.ContainerCollection.CloseContainer(pair.Key);

                                ctx.AddPacket(player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                            }
                        }
                        else if (pair.Value.Metadata.OpenTibiaId == 2594)
                        {
                            player.Client.ContainerCollection.CloseContainer(pair.Key);

                            ctx.AddPacket(player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                    }
                } );
            }

            return next(context);
        }
    }
}