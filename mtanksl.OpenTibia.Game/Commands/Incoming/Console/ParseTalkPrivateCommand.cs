using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkPrivateCommand : Command
    {
        public ParseTalkPrivateCommand(Player player, string name, string message)
        {
            Player = player;

            Name = name;

            Message = message;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Player observer = context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == Name)
                    .FirstOrDefault();

                if (observer != null && observer != Player)
                {
                    context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Private, Message) );

                    resolve(context);
                }
            } );
        }
    }
}