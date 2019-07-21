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

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Player observer = server.Map.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            //Act

            if (observer != null && observer != Player)
            {
                //Notify

                context.Write(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Private, Message) );
            }

            base.Execute(server, context);
        }
    }
}