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

        public override void Execute(Context context)
        {
            ISpeechScript script;

            if ( !Message.StartsWith("/") || !context.Server.Scripts.SpeechScripts.TryGetValue(GetCommand(Message), out script) || !script.OnSpeech(Player, GetParameters(Message), context) )
            {
                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Player.Name, Player.Level, TalkType.Say, Player.Tile.Position, Message) );
                    }
                }
            }

            base.OnCompleted(context);
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