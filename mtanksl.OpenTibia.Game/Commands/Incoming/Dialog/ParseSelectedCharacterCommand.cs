using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
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
            Connection.Keys = Packet.Keys;

            if (Packet.Version != 860)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.OnlyProtocol86Allowed) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbPlayer dbPlayer = Context.Database.PlayerRepository.GetAccountPlayer(Packet.Account, Packet.Password, Packet.Character);

            if (dbPlayer == null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.AccountNameOrPasswordIsNotCorrect) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbBan dbBan = Context.Database.BanRepository.GetBanByIpAddress(Connection.IpAddress);

            if (dbBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, dbBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            dbBan = Context.Database.BanRepository.GetBanByAccountId(dbPlayer.AccountId);

            if (dbBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, dbBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            dbBan = Context.Database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

            if (dbBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, dbBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            int position;

            byte time;

            if ( !Context.Server.WaitingList.CanLogin(dbPlayer.Id, out position, out time) )
            {
                Context.AddPacket(Connection, new OpenPleaseWaitDialogOutgoingPacket("Too many players online. You are at " + position + " place on the waiting list.", time) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            Tile toTile = null;

            Player onlinePlayer = Context.Server.GameObjects.GetPlayers()
                .Where(p => p.Name == dbPlayer.Name)
                .FirstOrDefault();

            return Promise.Run( () =>
            {
                if (onlinePlayer != null)
                {
                    return Context.AddCommand(new PlayerDestroyCommand(onlinePlayer) );
                }

                return Promise.Completed;

            } ).Then( () =>
            {
                toTile = Context.Server.Map.GetTile(new Position(dbPlayer.CoordinateX, dbPlayer.CoordinateY, dbPlayer.CoordinateZ) );

                return Context.AddCommand(new TileCreatePlayerCommand(toTile, Connection, dbPlayer) );

            } ).Then( (player) =>
            {
                Context.AddPacket(Connection, new SendInfoOutgoingPacket(player.Id, player.CanReportBugs) );

                Context.AddPacket(Connection, new SendTilesOutgoingPacket(Context.Server.Map, player.Client, toTile.Position) );

                Context.AddPacket(Connection, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );
                                
                Context.AddPacket(Connection, new SendStatusOutgoingPacket(player.Health, player.MaxHealth, player.Capacity, player.Experience, player.Level, player.LevelPercent, player.Mana, player.MaxMana, player.Skills.MagicLevel, player.Skills.MagicLevelPercent, player.Soul, player.Stamina) );

                Context.AddPacket(Connection, new SendSkillsOutgoingPacket(player.Skills.Fist, player.Skills.FistPercent, player.Skills.Club, player.Skills.ClubPercent, player.Skills.Sword, player.Skills.SwordPercent, player.Skills.Axe, player.Skills.AxePercent, player.Skills.Distance, player.Skills.DistancePercent, player.Skills.Shield, player.Skills.ShieldPercent, player.Skills.Fish, player.Skills.FishPercent) );

                Context.AddPacket(Connection, new SetSpecialConditionOutgoingPacket(SpecialCondition.None) );

                foreach (var pair in player.Inventory.GetIndexedContents() )
                {
                    Context.AddPacket(Connection, new SlotAddOutgoingPacket(pair.Key, (Item)pair.Value) );
                }

                foreach (var pair in player.Client.Vips.GetIndexed() )
                {
                    Context.AddPacket(Connection, new VipOutgoingPacket( (uint)pair.Key, pair.Value, false) );
                }

                return Promise.Completed;

            } ).Then( () =>
            {
                if (onlinePlayer == null)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                }

                return Promise.Completed;
            } );
        }
    }
}