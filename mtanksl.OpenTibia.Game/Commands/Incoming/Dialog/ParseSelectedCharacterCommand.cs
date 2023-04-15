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

        public override async Promise Execute()
        {
            Connection.Keys = Packet.Keys;

            if (Packet.Version != 860)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                Context.Disconnect(Connection);

                await Promise.Break;
            }
            else
            {
                Data.Models.Player databasePlayer = Context.DatabaseContext.PlayerRepository.GetAccountPlayer(Packet.Account, Packet.Password, Packet.Character);

                if (databasePlayer == null)
                {
                    Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                    Context.Disconnect(Connection);

                    await Promise.Break;
                }
                else
                {
                    Player onlinePlayer = Context.Server.GameObjects.GetPlayers()
                        .Where(p => p.Name == databasePlayer.Name)
                        .FirstOrDefault();

                    if (onlinePlayer != null)
                    {
                        await Context.AddCommand(new PlayerDestroyCommand(onlinePlayer) );
                    }

                    Tile toTile = Context.Server.Map.GetTile(new Position(databasePlayer.CoordinateX, databasePlayer.CoordinateY, databasePlayer.CoordinateZ) );

                    if (toTile == null)
                    {
                        await Promise.Break;
                    }
                    else
                    {
                        Player player = await Context.AddCommand(new TileCreatePlayerCommand(toTile, Connection, databasePlayer) );

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
                            await Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                        }
                    }
                }
            }
        }
    }
}