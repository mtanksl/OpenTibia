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

            if ( Message.StartsWith("/") )
            {
                int index = Message.IndexOf(' ');

                string command;

                string parameters;

                if (index == -1)
                {
                    command = Message;

                    parameters = "";
                }
                else
                {
                    command = Message.Substring(0, index);

                    parameters = Message.Substring(index + 1);
                }

                SpeechScript script;

                if ( !server.SpeechScripts.TryGetValue(command, out script) || !script.Execute(Player, parameters, server, context) )
                {
                    
                }
            }
            else
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