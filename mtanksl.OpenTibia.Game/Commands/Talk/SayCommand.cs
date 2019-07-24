using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

            if (Message.StartsWith("/i") )
            {
                ushort openTibiaId;

                if (ushort.TryParse(Message.Substring(3), out openTibiaId) )
                {
                    //Act

                    CreateItemCommand command = new CreateItemCommand(openTibiaId, Player.Tile.Position.Offset(Player.Direction) );

                    command.Completed += (s, e) =>
                    {
                        //Notify

                        base.Execute(e.Server, e.Context);
                    };

                    command.Execute(server, context);
                }                
            }
            else if ( Message.StartsWith("/m") )
            {
                //Act

                CreateMonsterCommand command = new CreateMonsterCommand(Message.Substring(3), Player.Tile.Position.Offset(Player.Direction) );

                command.Completed += (s, e) =>
                {
                    //Notify

                    base.Execute(e.Server, e.Context);
                };

                command.Execute(server, context);
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

                base.Execute(server, context);
            }
        }
    }
}