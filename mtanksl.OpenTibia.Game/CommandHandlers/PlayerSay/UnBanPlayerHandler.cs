using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Net;

namespace OpenTibia.Game.CommandHandlers
{
    public class UnBanPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override async Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/unban ") )
            {
                string parameter = command.Message.Substring(7);

                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    IPAddress ipAddress;

                    if (IPAddress.TryParse(parameter, out ipAddress) )
                    {
                        DbBan dbBan = await database.BanRepository.GetBanByIpAddress(ipAddress.ToString() );

                        if (dbBan != null)
                        {
                            database.BanRepository.RemoveBan(dbBan);

                            await database.Commit();
                        }

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "IP Address " + parameter + " has been unbanned.") );

                        return;
                    }
                    else
                    {
                        DbPlayer dbPlayer = await database.PlayerRepository.GetPlayerByName(parameter);

                        if (dbPlayer != null)
                        {
                            DbBan dbBan = await database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

                            if (dbBan != null)
                            {
                                database.BanRepository.RemoveBan(dbBan);

                                await database.Commit();
                            }

                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "Player " + parameter + " has been unbanned.") );

                            return;
                        }
                        else
                        {
                            DbAccount dbAccount = await database.AccountRepository.GetAccountByName(parameter);

                            if (dbAccount != null)
                            {
                                DbBan dbBan = await database.BanRepository.GetBanByAccountId(dbAccount.Id);

                                if (dbBan != null)
                                {
                                    database.BanRepository.RemoveBan(dbBan);

                                    await database.Commit();
                                }

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "Account " + parameter + " has been unbanned.") );

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