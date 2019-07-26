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

            SpeechScript script;

            if ( !Message.StartsWith("/") || Message.Length < 3 || !server.SpeechScripts.TryGetValue(Message.Substring(0, 2), out script) || !script.Execute(Player, Message.Substring(3), server, context) )
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
    }
}