using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class BroadcastMessageCommand : Command
    {
        public BroadcastMessageCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            //Act

            //Notify

            foreach (var observer in context.Server.Map.GetPlayers() )
            {
                context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Broadcast, Message) );
            }

            base.Execute(context);
        }
    }
}