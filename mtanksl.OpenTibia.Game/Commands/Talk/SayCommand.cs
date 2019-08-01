using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class SayCommand : Command
    {
        public SayCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            ISpeechScript script;

            if ( !Message.StartsWith("/") || !server.Scripts.SpeechScripts.TryGetValue(GetCommand(Message), out script) || !script.OnSpeech(Player, GetParameters(Message), server, context) )
            {
                //Notify

                foreach (var observer in server.Map.GetPlayers() )
                {
                    if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                    {
                        context.Write(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Say, Player.Tile.Position, Message) );
                    }
                }
            }

            base.Execute(server, context);
        }

        protected string GetCommand(string message)
        {
            int index = Message.IndexOf(' ');

            if (index == -1)
            {
                return message;
            }

            return message.Substring(0, index);
        }

        protected string GetParameters(string message)
        {
            int index = Message.IndexOf(' ');

            if (index == -1)
            {
                return "";
            }

            return message.Substring(index + 1);
        }
    }
}