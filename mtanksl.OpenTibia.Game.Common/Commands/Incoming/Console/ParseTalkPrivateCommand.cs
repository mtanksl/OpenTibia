using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkPrivateCommand : IncomingCommand
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

        public override Promise Execute()
        {
            // *<player>* <message>

            PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(Player);

            if (playerIdleBehaviour != null)
            {
                playerIdleBehaviour.SetLastAction();
            }

            PlayerMuteBehaviour playerChannelMuteBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerMuteBehaviour>(Player);

            if (playerChannelMuteBehaviour != null)
            {
                string message;

                if (playerChannelMuteBehaviour.IsMuted(out message) )
                {
                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, message) );

                    return Promise.Break;
                }      
            }

            Player observer = Context.Server.GameObjects.GetPlayerByName(Name);

            if (observer != null && observer != Player)
            {
                Context.AddPacket(observer, new ShowTextOutgoingPacket(Context.Server.Channels.GenerateStatementId(Player.DatabasePlayerId, Message), Player.Name, Player.Level, MessageMode.PrivateFrom, Message) );

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}