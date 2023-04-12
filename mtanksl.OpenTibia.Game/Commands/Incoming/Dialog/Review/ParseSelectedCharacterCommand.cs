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
                    Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                    Context.Disconnect(Connection);

                    resolve();
                }
                else
                {
                    var databasePlayer = Context.DatabaseContext.PlayerRepository.GetAccountPlayer(Packet.Account, Packet.Password, Packet.Character);

                    if (databasePlayer == null)
                    {
                        Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                        Context.Disconnect(Connection);

                        resolve();
                    }
                    else
                    {
                        var onlinePlayer = Context.Server.GameObjects.GetPlayers()
                            .Where(p => p.Name == databasePlayer.Name)
                            .FirstOrDefault();

                        if (onlinePlayer != null)
                        {
                            Context.AddCommand(new PlayerDestroyCommand(onlinePlayer) );

                            Context.Disconnect(onlinePlayer.Client.Connection);
                        }

                        Tile toTile = Context.Server.Map.GetTile(new Position(databasePlayer.CoordinateX, databasePlayer.CoordinateY, databasePlayer.CoordinateZ) );

                        if (toTile != null)
                        {
                            Context.AddCommand(new TileCreatePlayerCommand(toTile, Connection, databasePlayer) ).Then( (player) =>
                            {            
                                Context.AddPacket(Connection, new SendInfoOutgoingPacket(player.Id, player.CanReportBugs) );

                                Context.AddPacket(Connection, new SendTilesOutgoingPacket(Context.Server.Map, player.Client, toTile.Position) );

                                foreach (var pair in player.Inventory.GetIndexedContents() )
                                {
                                    Context.AddPacket(Connection, new SlotAddOutgoingPacket(pair.Key, (Item)pair.Value) );
                                }

                                Context.AddPacket(Connection, new SendStatusOutgoingPacket(player.Health, player.MaxHealth, player.Capacity, player.Experience, player.Level, player.LevelPercent, player.Mana, player.MaxMana, player.Skills.MagicLevel, player.Skills.MagicLevelPercent, player.Soul, player.Stamina) );

                                Context.AddPacket(Connection, new SendSkillsOutgoingPacket(player.Skills.Fist, player.Skills.FistPercent, player.Skills.Club, player.Skills.ClubPercent, player.Skills.Sword, player.Skills.SwordPercent, player.Skills.Axe, player.Skills.AxePercent, player.Skills.Distance, player.Skills.DistancePercent, player.Skills.Shield, player.Skills.ShieldPercent, player.Skills.Fish, player.Skills.FishPercent) );

                                Context.AddPacket(Connection, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );

                                Context.AddPacket(Connection, new SetSpecialConditionOutgoingPacket(SpecialCondition.None) );

                                foreach (var vip in player.Client.VipCollection.GetVips() )
                                {
                                    Context.AddPacket(Connection, new VipOutgoingPacket(vip.Id, vip.Name, false) );
                                }

                                if (onlinePlayer == null)
                                {
                                    Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                                }

                                resolve();
                            } );
                        }
                    }
                }                
            } );
        }
    }
}