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

        public override async Promise Execute()
        {
            Connection.Keys = Packet.Keys;

            if (Packet.TibiaDat != Constants.TibiaDat || Packet.TibiaPic != Constants.TibiaPic || Packet.TibiaSpr != Constants.TibiaSpr || Packet.Version != Constants.Version)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                Context.Disconnect(Connection);

                await Promise.Break; return;
            }

            if (Context.Server.Status != ServerStatus.Running && Connection.IpAddress != "127.0.0.1")
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.TibiaIsCurrentlyDownForMaintenance) );

                Context.Disconnect(Connection);

                await Promise.Break; return;
            }

            if ( !Context.Server.RateLimiting.IsLoginAttempsOk(Connection.IpAddress) )
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.TooManyLoginAttempts) );

                Context.Disconnect(Connection);

                await Promise.Break; return;
            }

            DbAccount dbAccount;

            DbBan dbBan;

            using (var database = Context.Server.DatabaseFactory.Create() )
            {
                dbBan = await database.BanRepository.GetBanByIpAddress(Connection.IpAddress);

                if (dbBan != null)
                {
                    Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, dbBan.Message) );

                    Context.Disconnect(Connection);

                    await Promise.Break; return;
                }

                bool isAccountManager = false;

                if (Context.Server.Config.LoginAccountManagerEnabled && 
                    Packet.Account == Context.Server.Config.LoginAccountManagerAccountName && 
                    Packet.Password == Context.Server.Config.LoginAccountManagerAccountPassword)
                {
                    isAccountManager = true;
                }

                if (isAccountManager)
                {
                    dbAccount = new DbAccount()
                    {
                        Name = Context.Server.Config.LoginAccountManagerAccountName,

                        Password = Context.Server.Config.LoginAccountManagerAccountPassword,

                        Players = new List<DbPlayer>(),

                        PremiumUntil = null
                    };
                }
                else
                {
                    dbAccount = await database.AccountRepository.GetAccount(Packet.Account, Packet.Password);

                    if (dbAccount == null)
                    {
                        Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                        Context.Disconnect(Connection);

                        await Promise.Break; return;
                    }

                    dbBan = await database.BanRepository.GetBanByAccountId(dbAccount.Id);

                    if (dbBan != null)
                    {
                        Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(true, dbBan.Message) );

                        Context.Disconnect(Connection);

                        await Promise.Break; return;
                    }
                }

                DbMotd dbMotd = await database.MotdRepository.GetLastMessageOfTheDay();

                if (dbMotd != null)
                {
                    Context.AddPacket(Connection, new OpenMessageOfTheDayDialogOutgoingPacket(dbMotd.Id, dbMotd.Message) );    
                }
            }

            if (Context.Server.Config.LoginAccountManagerEnabled)
            {
                dbAccount.Players.Insert(0, new DbPlayer()
                {
                    Name = Context.Server.Config.LoginAccountManagerPlayerName,

                    World = new DbWorld()
                    {
                        Name = Context.Server.Config.LoginAccountManagerWorldName,

                        Ip = Context.Server.Config.LoginAccountManagerIpAddress,

                        Port = Context.Server.Config.LoginAccountManagerPort
                    }
                } );
            }  

            List<CharacterDto> characters = new List<CharacterDto>();

            foreach (var dbPlayer in dbAccount.Players)
            {
                string worldName;

                if (Context.Server.Config.GameplayShowOnlineStatusInCharlist)
                {                
                    string playerName = Context.Server.GameObjectPool.GetPlayerNameFor(Connection.IpAddress, dbPlayer.Id, dbPlayer.Name);

                    Player onlinePlayer = Context.Server.GameObjects.GetPlayerByName(playerName);

                    if (onlinePlayer == null)
                    {
                        worldName = "Offline";
                    }
                    else
                    {
                        worldName = "Online";
                    }
                }
                else
                {
                    worldName = dbPlayer.World.Name;
                }

                characters.Add(new CharacterDto(dbPlayer.Name, worldName, dbPlayer.World.Ip, (ushort)dbPlayer.World.Port) );
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

            await Promise.Completed; return;
        }
    }
}