using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkPrivateRedCommand : IncomingCommand
    {
        public ParseTalkPrivateRedCommand(Player player, string name, string message)
        {
            Player = player;

            Name = name;

            Message = message;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // @<player>@ <message>

            if (Player.Rank == Rank.Gamemaster)
            {
                Player observer = Context.Server.GameObjects.GetPlayerByName(Name);

                if (observer != null && observer != Player)
                {
                    Context.AddPacket(observer, new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, MessageMode.GamemasterPrivateFrom, Message) );

                    return Promise.Completed;
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }    
}