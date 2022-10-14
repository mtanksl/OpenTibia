using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseSelectedCharacterCommand : Command
    {
        public ParseSelectedCharacterCommand(IConnection connection, SelectedCharacterIncomingPacket packet)
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

                                                          new SendTilesOutgoingPacket(ctx.Server.Map, client, toTile.Position),

                                                          new SendStatusOutgoingPacket(player.Health, player.MaxHealth, player.Capacity, player.Experience, player.Level, player.LevelPercent, player.Mana, player.MaxMana, player.Skills.MagicLevel, player.Skills.MagicLevelPercent, player.Soul, player.Stamina),

                                                          new SendSkillsOutgoingPacket(player.Skills.Fist, player.Skills.FistPercent, player.Skills.Club, player.Skills.ClubPercent, player.Skills.Sword, player.Skills.SwordPercent, player.Skills.Axe, player.Skills.AxePercent, player.Skills.Distance, player.Skills.DistancePercent, player.Skills.Shield, player.Skills.ShieldPercent, player.Skills.Fish, player.Skills.FishPercent),

                                                          new SetEnvironmentLightOutgoingPacket(ctx.Server.Clock.Light),

                                                          new SetSpecialConditionOutgoingPacket(SpecialCondition.None) );

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