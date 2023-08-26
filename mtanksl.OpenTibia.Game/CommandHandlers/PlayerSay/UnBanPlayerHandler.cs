﻿using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UnBanPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/unban ") && command.Player.Vocation == Vocation.Gamemaster)
            {
                string name = command.Message.Substring(7);

                DbPlayer databasePlayer = Context.Database.PlayerRepository.GetPlayerByName(name);

                if (databasePlayer != null)
                {
                    DbBan databaseBan = Context.Database.BanRepository.GetBanByPlayerId(databasePlayer.Id);

                    if (databaseBan != null)
                    {
                        Context.Database.BanRepository.RemoveBan(databaseBan);

                        Context.Database.Commit();
                    }

                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, databasePlayer.Name + " has been unbanned.") );

                    return Promise.Completed;
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}