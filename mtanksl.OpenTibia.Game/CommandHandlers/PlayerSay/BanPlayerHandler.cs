using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class BanPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/ban ") )
            {
                string name = command.Message.Substring(5);

                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    DbPlayer dbPlayer = database.PlayerRepository.GetPlayerByName(name);

                    if (dbPlayer != null)
                    {
                        DbBan dbBan = database.BanRepository.GetBanByPlayerId(dbPlayer.Id);

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

                            database.Commit();
                        }

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, dbPlayer.Name + " has been banned.") );

                        Player observer = Context.Server.GameObjects.GetPlayerByName(name);

                        if (observer != null)
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(observer, MagicEffectType.Puff) ).Then( () =>
                            {
                                return Context.AddCommand(new CreatureDestroyCommand(observer) );
                            } );
                        }
                    }
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}