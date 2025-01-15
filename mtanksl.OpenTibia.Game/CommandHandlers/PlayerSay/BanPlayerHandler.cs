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
                string name = command.Message.Substring(5);

                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    IPAddress ipAddress;

                    if (IPAddress.TryParse(name, out ipAddress) )
                    {
                        DbBan dbBan = await database.BanRepository.GetBanByIpAddress(name);

                        if (dbBan == null)
                        {
                            dbBan = new DbBan()
                            {
                                Type = BanType.IpAddress,

                                IpAddress = name,

                                Message = "This account has been banned by " + command.Player.Name + ".",

                                CreationDate = DateTime.UtcNow
                            };

                            database.BanRepository.AddBan(dbBan);

                            await database.Commit();
                        }

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "IP Address " + name + " has been banned.") );

                        foreach (var observer in Context.Server.GameObjects.GetPlayers().ToArray() )
                        {
                            if (observer.Client.Connection.IpAddress == name)
                            {
                                await Context.AddCommand(new ShowMagicEffectCommand(observer, MagicEffectType.Puff) );
                                
                                await Context.AddCommand(new CreatureDestroyCommand(observer) );
                            }
                        }

                        return;
                    }
                    else
                    {
                        DbPlayer dbPlayer = await database.PlayerRepository.GetPlayerByName(name);

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

                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Player " + name + " has been banned.") );

                            Player observer = Context.Server.GameObjects.GetPlayerByName(name);

                            if (observer != null)
                            {
                                await Context.AddCommand(new ShowMagicEffectCommand(observer, MagicEffectType.Puff) );
                                
                                await Context.AddCommand(new CreatureDestroyCommand(observer) );
                            }

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