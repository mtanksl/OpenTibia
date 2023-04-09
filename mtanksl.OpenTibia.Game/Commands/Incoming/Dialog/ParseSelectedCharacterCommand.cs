using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

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

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
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
                    var databasePlayer = context.DatabaseContext.PlayerRepository.GetAccountPlayer(Packet.Account, Packet.Password, Packet.Character);

                    if (databasePlayer == null)
                    {
                        context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                        context.Disconnect(Connection);

                        resolve(context);
                    }
                    else
                    {
                        var onlinePlayer = context.Server.GameObjects.GetPlayers()
                            .Where(p => p.Name == databasePlayer.Name)
                            .FirstOrDefault();

                        if (onlinePlayer != null)
                        {
                            context.AddCommand(new PlayerDestroyCommand(onlinePlayer) );

                            context.Disconnect(onlinePlayer.Client.Connection);
                        }

                        Tile toTile = context.Server.Map.GetTile(new Position(databasePlayer.CoordinateX, databasePlayer.CoordinateY, databasePlayer.CoordinateZ) );

                        if (toTile != null)
                        {
                            context.AddCommand(new TileCreatePlayerCommand(toTile, Connection, databasePlayer) ).Then( (ctx, player) =>
                            {            
                                ctx.AddPacket(Connection, new SendInfoOutgoingPacket(player.Id, player.CanReportBugs) );

                                ctx.AddPacket(Connection, new SendTilesOutgoingPacket(ctx.Server.Map, player.Client, toTile.Position) );

                                foreach (var pair in player.Inventory.GetIndexedContents() )
                                {
                                    ctx.AddPacket(Connection, new SlotAddOutgoingPacket(pair.Key, (Item)pair.Value) );
                                }

                                ctx.AddPacket(Connection, new SendStatusOutgoingPacket(player.Health, player.MaxHealth, player.Capacity, player.Experience, player.Level, player.LevelPercent, player.Mana, player.MaxMana, player.Skills.MagicLevel, player.Skills.MagicLevelPercent, player.Soul, player.Stamina) );

                                ctx.AddPacket(Connection, new SendSkillsOutgoingPacket(player.Skills.Fist, player.Skills.FistPercent, player.Skills.Club, player.Skills.ClubPercent, player.Skills.Sword, player.Skills.SwordPercent, player.Skills.Axe, player.Skills.AxePercent, player.Skills.Distance, player.Skills.DistancePercent, player.Skills.Shield, player.Skills.ShieldPercent, player.Skills.Fish, player.Skills.FishPercent) );

                                ctx.AddPacket(Connection, new SetEnvironmentLightOutgoingPacket(ctx.Server.Clock.Light) );

                                ctx.AddPacket(Connection, new SetSpecialConditionOutgoingPacket(SpecialCondition.None) );

                                foreach (var vip in player.Client.VipCollection.GetVips() )
                                {
                                    ctx.AddPacket(Connection, new VipOutgoingPacket(vip.Id, vip.Name, false) );
                                }

                                if (onlinePlayer == null)
                                {
                                    ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                                }

                                resolve(ctx);
                            } );
                        }
                    }
                }                
            } );
        }
    }
}