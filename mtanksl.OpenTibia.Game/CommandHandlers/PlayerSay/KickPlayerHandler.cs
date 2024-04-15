using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class KickPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/kick ") )
            {
                string name = command.Message.Substring(6);

                Player observer = Context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == name)
                    .FirstOrDefault();

                if (observer != null && observer != command.Player)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(observer, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new CreatureDestroyCommand(observer) );
                    } );
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}