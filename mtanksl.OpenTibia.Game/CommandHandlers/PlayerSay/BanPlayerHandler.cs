using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class BanPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/ban ") && command.Player.Rank == Rank.Gamemaster)
            {
                string name = command.Message.Substring(5);

                DbPlayer dbPlayer = Context.Database.PlayerRepository.GetPlayerByName(name);

                if (dbPlayer != null)
                {
                    DbBan dbBan = Context.Database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

                    if (dbBan == null)
                    {
                        dbBan = new DbBan()
                        {
                            Type = BanType.Player,

                            PlayerId = dbPlayer.Id,

                            Message = "This player has been banned by " + command.Player.Name + ".",

                            CreationDate = DateTime.UtcNow
                        };

                        Context.Database.BanRepository.AddBan(dbBan);

                        Context.Database.Commit();
                    }

                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, dbPlayer.Name + " has been banned.") );

                    Player observer = Context.Server.GameObjects.GetPlayers()
                        .Where(p => p.Name == name)
                        .FirstOrDefault();

                    if (observer != null)
                    {
                        return Context.AddCommand(new PlayerDestroyCommand(observer) );
                    }
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}