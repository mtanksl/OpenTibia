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
            if (command.Message.StartsWith("/ban ") && command.Player.Vocation == Vocation.Gamemaster)
            {
                string name = command.Message.Substring(5);

                DbPlayer databasePlayer = Context.Database.PlayerRepository.GetPlayerByName(name);

                if (databasePlayer != null)
                {
                    DbBan databaseBan = Context.Database.BanRepository.GetBanByPlayerId(databasePlayer.Id);

                    if (databaseBan == null)
                    {
                        databaseBan = new DbBan()
                        {
                            Type = BanType.Player,

                            PlayerId = databasePlayer.Id,

                            Message = "This player has been banned by " + command.Player.Name + "."
                        };

                        Context.Database.BanRepository.AddBan(databaseBan);

                        Context.Database.Commit();
                    }

                    Context.AddPacket(command.Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, databasePlayer.Name + " has been banned.") );

                    Player observer = Context.Server.GameObjects.GetPlayers()
                        .Where(p => p.Name == name)
                        .FirstOrDefault();

                    if (observer != null)
                    {
                        return Context.AddCommand(new ParseLogOutCommand(observer) );
                    }

                    return Promise.Completed;
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}