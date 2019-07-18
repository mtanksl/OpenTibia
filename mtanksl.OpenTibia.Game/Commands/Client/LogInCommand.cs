using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class LogInCommand : Command
    {
        private static int sequence;

        public LogInCommand(IConnection connection, SelectedCharacterIncomingPacket packet)
        {
            Connection = connection;

            Packet = packet;
        }

        public IConnection Connection { get; set; }

        public SelectedCharacterIncomingPacket Packet { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            Connection.Keys = Packet.Keys;

            if (Packet.Version != 860)
            {
                context.Write(Connection, new OpenSorryDialog(true, Constants.OnlyProtocol86Allowed) );
            }
            else
            {
                var account = new Data.PlayerRepository().GetPlayer(Packet.Account, Packet.Password, Packet.Character);

                if (account == null)
                {
                    context.Write(Connection, new OpenSorryDialog(true, Constants.AccountNameOrPasswordIsNotCorrect) );
                }
                else
                {
                    Connection.Client = new Client(server)
                    {
                        Player = new Player()
                        {
                            Name = account.Name + " " + sequence++
                        }
                    };

                    //Arrange

                    IClient client = Connection.Client;

                    Player player = client.Player;

                    Position position = new Position(account.CoordinateX, account.CoordinateY, account.CoordinateZ);

                    Tile fromTile = server.Map.GetTile(position);

                    //Act

                    server.Map.AddCreature(player);

                    byte fromIndex = fromTile.AddContent(player);

                    //Notify

                    foreach (var observer in server.Map.GetPlayers() )
                    {
                        if (observer != player)
                        {
                            if (observer.Tile.Position.CanSee(position) )
                            {
                                uint removeId;

                                if (observer.Client.IsKnownCreature(player.Id, out removeId) )
                                {
                                    context.Write(observer.Client.Connection, new ThingAdd(position, fromIndex, player) )

                                           .Write(observer.Client.Connection, new ShowMagicEffect(position, MagicEffectType.Teleport) );
                                }
                                else
                                {
                                    context.Write(observer.Client.Connection, new ThingAdd(position, fromIndex, removeId, player) )

                                           .Write(observer.Client.Connection, new ShowMagicEffect(position, MagicEffectType.Teleport) );
                                }
                            }
                        }
                    }

                    context.Write(Connection, new SendInfo(player.Id, player.CanReportBugs) )  
                           
                           .Write(Connection, new SetSpecialCondition(SpecialCondition.None) )            
                           
                           .Write(Connection, new SendStatus(player.Health, player.MaxHealth, player.Capacity, player.Experience, player.Level, player.LevelPercent, player.Mana, player.MaxMana, 0, 0, player.Soul, player.Stamina) )            
                           
                           .Write(Connection, new SendSkills(10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0) )                
                           
                           .Write(Connection, new SetEnvironmentLight(Light.Day) )            
                           
                           .Write(Connection, new SendTiles(server.Map, client, position) )            
                           
                           .Write(Connection, new ShowMagicEffect(position, MagicEffectType.Teleport) );
                }
            }
        }
    }
}