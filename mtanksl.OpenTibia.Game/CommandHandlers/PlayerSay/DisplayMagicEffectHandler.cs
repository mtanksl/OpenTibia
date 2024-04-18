using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class DisplayMagicEffectHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            int id;

            if (command.Message.StartsWith("/me ") && int.TryParse(command.Message.Substring(4), out id) && id >= 1 && id <= 70)
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, (MagicEffectType)id) );
            }

            return next();
        }
    }
}