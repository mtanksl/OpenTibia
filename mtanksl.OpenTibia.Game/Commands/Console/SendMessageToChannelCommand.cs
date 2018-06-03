using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class SendMessageToChannel : Command
    {
        private Server server;

        public SendMessageToChannel(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Channel channel = server.Channels.GetChannel(ChannelId);

            //Act
                        
            if (channel != null)
            {
                if (channel.ContainsPlayer(Player) )
                {
                    foreach (var observer in channel.GetPlayers() )
                    {
                        //Notify

                        context.Response.Write(observer.Client.Connection, new ShowText(0, Player.Name, Player.Level, TalkType.ChannelYellow, channel.Id, Message) );
                    }
                }
            }
        }
    }
}