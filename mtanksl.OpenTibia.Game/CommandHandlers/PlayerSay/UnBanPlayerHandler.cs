using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UnBanPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/unban ") )
            {
                string name = command.Message.Substring(7);

                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    DbPlayer dbPlayer = database.PlayerRepository.GetPlayerByName(name);

                    if (dbPlayer != null)
                    {
                        DbBan dbBan = database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

                        if (dbBan != null)
                        {
                            database.BanRepository.RemoveBan(dbBan);

                            database.Commit();
                        }

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, dbPlayer.Name + " has been unbanned.") );

                        return Promise.Completed;
                    }
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}