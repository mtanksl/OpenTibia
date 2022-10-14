using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkBroadcastCommand : Command
    {
        public ParseTalkBroadcastCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Broadcast, Message) );
                }

                resolve(context);
            } );
        }
    }
}