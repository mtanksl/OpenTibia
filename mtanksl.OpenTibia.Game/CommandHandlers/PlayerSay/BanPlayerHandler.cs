using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;
using System.Net;

namespace OpenTibia.Game.CommandHandlers
{
    public class BanPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override async Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/ban ") )
            {
                string parameter = command.Message.Substring(5);

                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    IPAddress ipAddress;

                    if (IPAddress.TryParse(parameter, out ipAddress) )
                    {
                        DbBan dbBan = await database.BanRepository.GetBanByIpAddress(ipAddress.ToString() );

                        if (dbBan == null)
                        {
                            dbBan = new DbBan()
                            {
                                Type = BanType.IpAddress,

                                IpAddress = ipAddress.ToString(),

                                Message = "This account has been banned by " + command.Player.Name + ".",

                                CreationDate = DateTime.UtcNow
                            };

                            database.BanRepository.AddBan(dbBan);

                            await database.Commit();
                        }

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "IP Address " + parameter + " has been banned.") );

                        foreach (var observer in Context.Server.GameObjects.GetPlayersByIpAddress(ipAddress.ToString() ).ToArray() )
                        {
                            await Context.AddCommand(new ShowMagicEffectCommand(observer, MagicEffectType.Puff) );
                                
                            await Context.AddCommand(new CreatureDestroyCommand(observer) );
                        }

                        return;
                    }
                    else
                    {
                        DbPlayer dbPlayer = await database.PlayerRepository.GetPlayerByName(parameter);

                        if (dbPlayer != null)
                        {
                            DbBan dbBan = await database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

                            if (dbBan == null)
                            {
                                dbBan = new DbBan()
                                {
                                    Type = BanType.Player,

                                    PlayerId = dbPlayer.Id,

                                    Message = "This player has been banned by " + command.Player.Name + ".",

                                    CreationDate = DateTime.UtcNow
                                };

                                database.BanRepository.AddBan(dbBan);

                                await database.Commit();
                            }

                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Player " + parameter + " has been banned.") );

                            Player observer = Context.Server.GameObjects.GetPlayerByName(dbPlayer.Name);

                            if (observer != null)
                            {
                                await Context.AddCommand(new ShowMagicEffectCommand(observer, MagicEffectType.Puff) );
                                
                                await Context.AddCommand(new CreatureDestroyCommand(observer) );
                            }

                            return;
                        }
                        else
                        {
                            DbAccount dbAccount = await database.AccountRepository.GetAccountByName(parameter);

                            if (dbAccount != null)
                            {
                                DbBan dbBan = await database.BanRepository.GetBanByAccountId(dbAccount.Id);

                                if (dbBan == null)
                                {
                                    dbBan = new DbBan()
                                    {
                                        Type = BanType.Account,

                                        AccountId = dbAccount.Id,

                                        Message = "This account has been banned by " + command.Player.Name + ".",

                                        CreationDate = DateTime.UtcNow
                                    };

                                    database.BanRepository.AddBan(dbBan);

                                    await database.Commit();
                                }

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Account " + parameter + " has been banned.") );

                                foreach (var observer in Context.Server.GameObjects.GetPlayersByAccount(dbAccount.Id).ToArray() )
                                {
                                    await Context.AddCommand(new ShowMagicEffectCommand(observer, MagicEffectType.Puff) );
                                
                                    await Context.AddCommand(new CreatureDestroyCommand(observer) );
                                }

                                return;
                            }
                        }
                    }
                }

                await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) ); return;
            }

            await next(); return;
        }
    }
}