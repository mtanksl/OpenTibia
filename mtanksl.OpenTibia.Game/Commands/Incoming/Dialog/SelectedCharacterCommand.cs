using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class SelectedCharacterCommand : Command
    {
        public SelectedCharacterCommand(IConnection connection, SelectedCharacterIncomingPacket packet)
        {
            Connection = connection;

            Packet = packet;
        }

        public IConnection Connection { get; set; }

        public SelectedCharacterIncomingPacket Packet { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Connection.Keys = Packet.Keys;

                if (Packet.Version != 860)
                {
                    context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                    context.Disconnect(Connection);

                    resolve(context);
                }
                else
                {
                    var account = context.DatabaseContext.PlayerRepository.GetPlayer(Packet.Account, Packet.Password, Packet.Character);

                    if (account == null)
                    {
                        context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                        context.Disconnect(Connection);

                        resolve(context);
                    }
                    else
                    {
                        Tile toTile = context.Server.Map.GetTile(new Position(account.CoordinateX, account.CoordinateY, account.CoordinateZ) );

                        if (toTile != null)
                        {
                            context.AddCommand(new TileCreatePlayerCommand(toTile, account.Name) ).Then( (ctx, player) =>
                            {
                                Client client = new Client(ctx.Server);

                                    client.Player = player;

                                    Connection.Client = client;
                                               
                                ctx.AddPacket(Connection, new SendInfoOutgoingPacket(player.Id, player.CanReportBugs), 

                                                          new SetSpecialConditionOutgoingPacket(SpecialCondition.None),
                                                    
                                                          new SendStatusOutgoingPacket(player.Health, player.MaxHealth, player.Capacity, player.Experience, player.Level, player.LevelPercent, player.Mana, player.MaxMana, 0, 0, player.Soul, player.Stamina),
                                                    
                                                          new SendSkillsOutgoingPacket(10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0),
                                                    
                                                          new SetEnvironmentLightOutgoingPacket(ctx.Server.Clock.Light),
                                                    
                                                          new SendTilesOutgoingPacket(ctx.Server.Map, client, toTile.Position) );

                                ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                                resolve(ctx);
                            } );
                        }
                    }
                }                
            } );
        }
    }
}