using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseEnterGameCommand : IncomingCommand
    {
        public ParseEnterGameCommand(IConnection connection, EnterGameIncomingPacket packet)
        {
            Connection = connection;

            Packet = packet;
        }

        public IConnection Connection { get; set; }

        public EnterGameIncomingPacket Packet { get; set; }

        public override Promise Execute()
        {
            Connection.Keys = Packet.Keys;

            if (Packet.TibiaDat != 1277983123 || Packet.TibiaPic != 1256571859 || Packet.TibiaSpr != 1277298068 || Packet.Version != 860)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            if (Context.Server.Status != ServerStatus.Running && Connection.IpAddress != "127.0.0.1")
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.TibiaIsCurrentlyDownForMaintenance) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            if ( !Context.Server.RateLimiting.IsLoginAttempsOk(Connection.IpAddress) )
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.TooManyLoginAttempts) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbAccount dbAccount = Context.Server.Database.PlayerRepository.GetAccount(Packet.Account, Packet.Password);

            if (dbAccount == null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbBan dbBan = Context.Server.Database.BanRepository.GetBanByIpAddress(Connection.IpAddress);

            if (dbBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, dbBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            dbBan = Context.Server.Database.BanRepository.GetBanByAccountId(dbAccount.Id);

            if (dbBan != null)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, dbBan.Message) );

                Context.Disconnect(Connection);

                return Promise.Break;
            }

            DbMotd dbMotd = Context.Server.Database.MotdRepository.GetLastMessageOfTheDay();

            if (dbMotd != null)
            {
                Context.AddPacket(Connection, new OpenMessageOfTheDayDialogOutgoingPacket(dbMotd.Id, dbMotd.Message) );    
            }

            List<CharacterDto> characters = new List<CharacterDto>();

            foreach (var player in dbAccount.Players)
            {
                characters.Add( new CharacterDto(player.Name, player.World.Name, player.World.Ip, (ushort)player.World.Port) );
            }

            int premiumDays;

            if (dbAccount.PremiumUntil != null)
            {
                premiumDays = (int)Math.Ceiling( (dbAccount.PremiumUntil.Value - DateTime.UtcNow).TotalDays );

                if (premiumDays > 0)
                {
                    if (premiumDays > ushort.MaxValue)
                    {
                        premiumDays = ushort.MaxValue;
                    }
                }
                else
                {
                    premiumDays = 0;
                }
            }
            else
            {
                premiumDays = 0;
            }

            Context.AddPacket(Connection, new OpenSelectCharacterDialogOutgoingPacket(characters, (ushort)premiumDays) );

            Context.Disconnect(Connection);

            return Promise.Completed;
        }
    }
}