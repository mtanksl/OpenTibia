using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class LogOutCommand : Command
    {
        public LogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            Position fromPosition = fromTile.Position;

            //Act

            server.Map.RemoveCreature(Player);

            fromTile.RemoveContent(fromIndex);

            //Clear

            server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player));

            Player.Client.ContainerCollection.Clear();

            Player.Client.WindowCollection.Clear();

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != Player)
                {
                    if (observer.Tile.Position.CanSee(fromPosition) )
                    {
                        context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex),

                                                                  new ShowMagicEffectOutgoingPacket(fromPosition, MagicEffectType.Puff) );
                    }
                }
            }

            context.Disconnect(Player.Client.Connection);

            base.Execute(server, context);
        }
    }
}