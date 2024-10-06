using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseSelectedCharacterCommand : IncomingCommand
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

            if (Context.Server.Status != ServerStatus.Running && Connection.IpAddress != "127.0.0.1")
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.TibiaIsCurrentlyDownForMaintenance) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            if ( !Context.Server.RateLimiting.IsLoginAttempsOk(Connection.IpAddress) )
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.TooManyLoginAttempts) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbPlayer dbPlayer = Context.Server.Database.PlayerRepository.GetAccountPlayer(Packet.Account, Packet.Password, Packet.Character);

            if (dbPlayer == null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.AccountNameOrPasswordIsNotCorrect) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbBan dbBan = Context.Server.Database.BanRepository.GetBanByIpAddress(Connection.IpAddress);

            if (dbBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, dbBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            dbBan = Context.Server.Database.BanRepository.GetBanByAccountId(dbPlayer.AccountId);

            if (dbBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, dbBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            dbBan = Context.Server.Database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

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

            Player onlinePlayer = Context.Server.GameObjects.GetPlayerByName(dbPlayer.Name);

            if (onlinePlayer != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.YouAreAlreadyLoggedIn) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            return Context.AddCommand(new TileCreatePlayerCommand(Connection, dbPlayer) ).Then( (player) =>
            {
                return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Teleport) );
            } );
        }
    }
}