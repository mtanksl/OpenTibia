﻿using OpenTibia.Common.Objects;
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

        public override async Promise Execute()
        {
            Connection.Keys = Packet.Keys;

            if (Packet.Version != 860)
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.OnlyProtocol86Allowed) );

                Context.Disconnect(Connection);

                await Promise.Break; return;
            }

            if (Context.Server.Status != ServerStatus.Running && Connection.IpAddress != "127.0.0.1")
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.TibiaIsCurrentlyDownForMaintenance) );

                Context.Disconnect(Connection);

                await Promise.Break; return;
            }

            if ( !Context.Server.RateLimiting.IsLoginAttempsOk(Connection.IpAddress) )
            {
                Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.TooManyLoginAttempts) );

                Context.Disconnect(Connection);

                await Promise.Break; return;
            }

            DbPlayer dbPlayer;

            DbBan dbBan;

            using (var database = Context.Server.DatabaseFactory.Create() )
            {
                dbBan = await database.BanRepository.GetBanByIpAddress(Connection.IpAddress);

                if (dbBan != null)
                {
                    Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, dbBan.Message) );

                    Context.Disconnect(Connection);

                    await Promise.Break; return;
                }

                if (Context.Server.Config.LoginAccountManagerEnabled && Packet.Character == Context.Server.Config.LoginAccountManagerPlayerName)
                {
                    dbPlayer = new DbPlayer() 
                    { 
                        Account = new DbAccount()
                        { 
                            PremiumUntil = null
                        },
                         
                        Name = Context.Server.Config.LoginAccountManagerPlayerName, 
                        
                        Health = 150, 
                        
                        MaxHealth = 150, 
                        
                        Direction = 2, 
                         
                        BaseOutfitItemId = 2031, 
                        
                        OutfitItemId = 2031, 
                        
                        BaseSpeed = 220,
                        
                        Speed = 220, 
                        
                        Experience = 0, 
                        
                        Level = 1, 
                        
                        Mana = 55,
                        
                        MaxMana = 55, 
                        
                        Soul = 100, 
                        
                        Capacity = 40000, 
                        
                        Stamina = 2520, 

                        Rank = 3,

                        SpawnX = Context.Server.Config.LoginAccountManagerPlayerPosition.X, 
                        
                        SpawnY = Context.Server.Config.LoginAccountManagerPlayerPosition.Y,
                        
                        SpawnZ = Context.Server.Config.LoginAccountManagerPlayerPosition.Z, 
                        
                        TownX = Context.Server.Config.LoginAccountManagerPlayerPosition.X, 
                        
                        TownY = Context.Server.Config.LoginAccountManagerPlayerPosition.Y, 
                        
                        TownZ = Context.Server.Config.LoginAccountManagerPlayerPosition.Z
                    };
                }
                else
                {
                    dbPlayer = await database.PlayerRepository.GetAccountPlayer(Packet.Account, Packet.Password, Packet.Character);

                    if (dbPlayer == null)
                    {
                        Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.AccountNameOrPasswordIsNotCorrect) );

                        Context.Disconnect(Connection);

                        await Promise.Break; return;
                    }

                    dbBan = await database.BanRepository.GetBanByAccountId(dbPlayer.AccountId);

                    if (dbBan != null)
                    {
                        Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, dbBan.Message) );

                        Context.Disconnect(Connection);

                        await Promise.Break; return;
                    }

                    dbBan = await database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

                    if (dbBan != null)
                    {
                        Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, dbBan.Message) );

                        Context.Disconnect(Connection);

                        await Promise.Break; return;
                    }
                }
            }

            int position;

            byte time;

            if ( !Context.Server.WaitingList.CanLogin(dbPlayer.Id, out position, out time) )
            {
                Context.AddPacket(Connection, new OpenPleaseWaitDialogOutgoingPacket("Too many players online. You are at " + position + " place on the waiting list.", time) );

                Context.Disconnect(Connection);

                await Promise.Break; return;
            }

            Player onlinePlayer = Context.Server.GameObjects.GetPlayerByName(dbPlayer.Name);

            if ( !Context.Server.Config.GameplayReplaceKickOnLogin)
            {
                if (onlinePlayer != null)
                {
                    Context.AddPacket(Connection, new OpenSorryDialogOutgoingPacket(false, Constants.YouAreAlreadyLoggedIn) );

                    Context.Disconnect(Connection);

                    await Promise.Break; return;
                }                
            }
            else
            {               
                if (onlinePlayer != null)
                {
                    await Context.AddCommand(new ShowMagicEffectCommand(onlinePlayer, MagicEffectType.Puff) );
                    
                    await Context.AddCommand(new CreatureDestroyCommand(onlinePlayer) );

                    await Promise.Yield();
                }
            }

            var player = await Context.AddCommand(new TileCreatePlayerCommand(Connection, dbPlayer) );
            
                         await Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Teleport) );
        }
    }
}