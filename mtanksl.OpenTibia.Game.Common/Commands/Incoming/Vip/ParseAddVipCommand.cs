﻿using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseAddVipCommand : IncomingCommand
    {
        public ParseAddVipCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute()
        {
            DbPlayer dbPlayer = Context.Database.PlayerRepository.GetPlayerByName(Name);

            if (dbPlayer != null && dbPlayer.Id != Player.DatabasePlayerId)
            {
                if (Player.Vips.Count < Context.Server.Config.GameplayMaxVips)
                {
                    if (Player.Vips.AddVip(dbPlayer.Id, dbPlayer.Name) )
                    {
                        Context.AddPacket(Player, new VipOutgoingPacket( (uint)dbPlayer.Id, dbPlayer.Name, Context.Server.GameObjects.GetPlayerByName(dbPlayer.Name) != null) );

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}