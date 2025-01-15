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
                string name = command.Message.Substring(7);

                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    IPAddress ipAddress;

                    if (IPAddress.TryParse(name, out ipAddress) )
                    {
                        DbBan dbBan = await database.BanRepository.GetBanByIpAddress(name);

                        if (dbBan != null)
                        {
                            database.BanRepository.RemoveBan(dbBan);

                            await database.Commit();
                        }

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "IP Address " + name + " has been unbanned.") );

                        return;
                    }
                    else
                    {
                        DbPlayer dbPlayer = await database.PlayerRepository.GetPlayerByName(name);

                        if (dbPlayer != null)
                        {
                            DbBan dbBan = await database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

                            if (dbBan != null)
                            {
                                database.BanRepository.RemoveBan(dbBan);

                                await database.Commit();
                            }

                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Player " + name + " has been unbanned.") );

                            return;
                        }
                    }
                }

                await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) ); return;
            }

            await next(); return;
        }
    }
}