using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class LogInCommand : Command
    {
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
                context.Write(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );
            }
            else
            {
                var account = new PlayerRepository().GetPlayer(Packet.Account, Packet.Password, Packet.Character);

                if (account == null)
                {
                    context.Write(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );
                }
                else
                {
                    //Arrange

                    Position fromPosition = new Position(account.CoordinateX, account.CoordinateY, account.CoordinateZ);

                    Tile fromTile = server.Map.GetTile(fromPosition);

                    if (fromTile != null)
                    {
                        //Act

                        Player player = new Player()
                        {
                            Name = account.Name
                        };

                        Client client = new Client(server)
                        {
                            Player = player
                        };

                        Connection.Client = client;

                        server.Map.AddCreature(player);

                        byte fromIndex = fromTile.AddContent(player);

                        //Notify

                        foreach (var observer in server.Map.GetPlayers() )
                        {
                            if (observer != player)
                            {
                                if (observer.Tile.Position.CanSee(fromPosition) )
                                {
                                    uint removeId;

                                    if (observer.Client.CreatureCollection.IsKnownCreature(player.Id, out removeId) )
                                    {
                                        context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(fromPosition, fromIndex, player),

                                                                                  new ShowMagicEffectOutgoingPacket(fromPosition, MagicEffectType.Teleport) );
                                    }
                                    else
                                    {
                                        context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(fromPosition, fromIndex, removeId, player),

                                                                                  new ShowMagicEffectOutgoingPacket(fromPosition, MagicEffectType.Teleport) );
                                    }
                                }
                            }
                        }

                        context.Write(Connection, new SendInfoOutgoingPacket(player.Id, player.CanReportBugs), 
                           
                                                  new SetSpecialConditionOutgoingPacket(SpecialCondition.None),
                                              
                                                  new SendStatusOutgoingPacket(player.Health, player.MaxHealth, player.Capacity, player.Experience, player.Level, player.LevelPercent, player.Mana, player.MaxMana, 0, 0, player.Soul, player.Stamina),
                                              
                                                  new SendSkillsOutgoingPacket(10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0),
                                              
                                                  new SetEnvironmentLightOutgoingPacket(Light.Day),
                                              
                                                  new SendTilesOutgoingPacket(server.Map, client, fromPosition),
                                              
                                                  new ShowMagicEffectOutgoingPacket(fromPosition, MagicEffectType.Teleport) );
                    }
                }
            }
        }
    }
}