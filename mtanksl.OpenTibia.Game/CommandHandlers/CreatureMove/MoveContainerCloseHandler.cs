using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveContainerCloseHandler : CommandHandler<CreatureMoveCommand>
    {
        public override bool CanHandle(Context context, CreatureMoveCommand command)
        {
            if (command.Creature is Player player && player.Client.ContainerCollection.GetContainers().Any(c => c.Root() is Tile) && !command.Data.ContainsKey("MoveContainerCloseHandler") )
            {
                command.Data.Add("MoveContainerCloseHandler", true);

                return true;
            }

            return false;
        }

        public override void Handle(Context context, CreatureMoveCommand command)
        {
            context.AddCommand(command).Then(ctx =>
            {
                Player player = (Player)command.Creature;

                foreach (var pair in player.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value.Root() is Tile tile && !command.ToTile.Position.IsNextTo(tile.Position) )
                    {
                        player.Client.ContainerCollection.CloseContainer(pair.Key);

                        ctx.AddPacket(player.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                    }
                }

                OnComplete(ctx);
            } );
        }
    }
}