using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class SendMessageToPlayerCommand : Command
    {
        public SendMessageToPlayerCommand(Player player, string name, string message)
        {
            Player = player;

            Name = name;

            Message = message;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Player observer = context.Server.Map.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            if (observer != null && observer != Player)
            {
                //Act

                //Notify

                context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Private, Message) );

                base.Execute(context);
            }
        }
    }
}