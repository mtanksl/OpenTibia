using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data;
using OpenTibia.Game.Components;
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

        public override void Execute(Context context)
        {
            if (Connection.Keys == null)
            {
                Connection.Keys = Packet.Keys;

                if (Packet.Version != 860)
                {
                    context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                    context.Disconnect(Connection);
                }
                else
                {
                    var account = new PlayerRepository().GetPlayer(Packet.Account, Packet.Password, Packet.Character);

                    if (account == null)
                    {
                        context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                        context.Disconnect(Connection);
                    }
                    else
                    {
                        Position toPosition = new Position(account.CoordinateX, account.CoordinateY, account.CoordinateZ);

                        Tile toTile = context.Server.Map.GetTile(toPosition);

                        if (toTile != null)
                        {
                            Player player = new Player()
                            {
                                Name = account.Name
                            };

                            Connection.Client = new Client(context.Server)
                            {
                                Player = player
                            };

                            context.Server.GameObjects.AddGameObject(player);

                            byte toIndex = toTile.AddContent(player);

                            foreach (var observer in context.Server.GameObjects.GetPlayers() )
                            {
                                if (observer == player)
                                {
                                    context.AddPacket(observer.Client.Connection, new SendInfoOutgoingPacket(player.Id, player.CanReportBugs), 

                                                                              new SetSpecialConditionOutgoingPacket(SpecialCondition.None),

                                                                              new SendStatusOutgoingPacket(player.Health, player.MaxHealth, 

                                                                                                           player.Capacity, 
                                                                                                          
                                                                                                           player.Experience, player.Level, player.LevelPercent, 
                                                                                                          
                                                                                                           player.Mana, player.MaxMana, 0, 0, player.Soul,
                                                                                                          
                                                                                                           player.Stamina),

                                                                              new SendSkillsOutgoingPacket(10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0),

                                                                              new SetEnvironmentLightOutgoingPacket(context.Server.Clock.Light),

                                                                              new SendTilesOutgoingPacket(context.Server.Map, player.Client, toPosition),

                                                                              new ShowMagicEffectOutgoingPacket(toPosition, MagicEffectType.Teleport) );
                                }
                                else
                                {
                                    if (observer.Tile.Position.CanSee(toPosition) )
                                    {
                                        uint removeId;

                                        if (observer.Client.CreatureCollection.IsKnownCreature(player.Id, out removeId) )
                                        {
                                            context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, player),

                                                                                      new ShowMagicEffectOutgoingPacket(toPosition, MagicEffectType.Teleport) );
                                        }
                                        else
                                        {
                                            context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, removeId, player),

                                                                                      new ShowMagicEffectOutgoingPacket(toPosition, MagicEffectType.Teleport) );
                                        }
                                    }
                                }                                
                            }

                            foreach (var component in player.GetComponents<Behaviour>() )
                            {
                                component.Start(context.Server);
                            }

                            context.AddEvent(new TileAddCreatureEventArgs(toTile, player, toIndex) );
                            
                            base.OnCompleted(context);
                        }
                    }
                }
            }
        }
    }
}