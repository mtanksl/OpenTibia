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

            DbPlayer databasePlayer = Context.DatabaseContext.PlayerRepository.GetAccountPlayer(Packet.Account, Packet.Password, Packet.Character);

            if (databasePlayer == null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.AccountNameOrPasswordIsNotCorrect) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbBan databaseBan = Context.DatabaseContext.BanRepository.GetBanByIpAddress(Connection.IpAddress);

            if (databaseBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, databaseBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            databaseBan = Context.DatabaseContext.BanRepository.GetBanByAccountId(databasePlayer.AccountId);

            if (databaseBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, databaseBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            databaseBan = Context.DatabaseContext.BanRepository.GetBanByPlayerId(databasePlayer.Id);

            if (databaseBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, databaseBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            int position;

            byte time;

            if ( !Context.Server.WaitingList.CanLogin(databasePlayer.Id, out position, out time) )
            {
                Context.AddPacket(Connection, new OpenPleaseWaitDialogOutgoingPacket("Too many players online. You are at " + position + " place on the waiting list.", time) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            Tile toTile = null;

            Player onlinePlayer = Context.Server.GameObjects.GetPlayers()
                .Where(p => p.Name == databasePlayer.Name)
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
                toTile = Context.Server.Map.GetTile(new Position(databasePlayer.CoordinateX, databasePlayer.CoordinateY, databasePlayer.CoordinateZ) );

                return Context.AddCommand(new TileCreatePlayerCommand(toTile, Connection, databasePlayer) );

            } ).Then( (player) =>
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