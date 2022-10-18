using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveAwayContainerCloseHandler : CommandHandler<CreatureUpdateParentCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, CreatureUpdateParentCommand command)
        {
            if (command.Creature is Player player)
            {
                return next(context).Then(ctx =>
                {
                    foreach (var pair in player.Client.ContainerCollection.GetIndexedContainers() )
                    {
                        IContainer root = pair.Value.Root();

                        if (root == null)
                        {
                            player.Client.ContainerCollection.CloseContainer(pair.Key);

                            ctx.AddPacket(player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                        }
                        else if (root is Tile tile)
                        {
                            if ( !command.ToTile.Position.IsNextTo(tile.Position) )
                            {
                                player.Client.ContainerCollection.CloseContainer(pair.Key);

                                ctx.AddPacket(player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                            }
                        }
                    }
                } );
            }

            return next(context);
        }
    }
}