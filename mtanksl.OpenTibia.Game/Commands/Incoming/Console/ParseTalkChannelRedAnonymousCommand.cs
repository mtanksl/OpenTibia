using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkChannelRedAnonymousCommand : Command
    {
        public ParseTalkChannelRedAnonymousCommand(Player player, ushort channelId, string message)
        {
            Player = player;

            ChannelId = channelId;

            Message = message;
        }

        public Player Player { get; set; }

        public ushort ChannelId { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // #d <message>

            if (Player.Rank == Rank.Gamemaster)
            {
                Channel channel = Context.Server.Channels.GetChannel(ChannelId);

                if (channel != null)
                {
                    if (channel.ContainsPlayer(Player) )
                    {                                                           
                        foreach (var observer in channel.GetPlayers() )
                        {
                            Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, TalkType.ChannelRedAnonymous, channel.Id, Message) );
                        }

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}