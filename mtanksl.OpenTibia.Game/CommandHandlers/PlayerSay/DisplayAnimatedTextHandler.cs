using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class DisplayAnimatedTextHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            int id;

            if (command.Message.StartsWith("/at ") && int.TryParse(command.Message.Substring(4), out id) && id >= 1 && id <= 255)
            {
                return Context.AddCommand(new ShowAnimatedTextCommand(command.Player, (AnimatedTextColor)id, 123456) );
            }

            return next();
        }
    }
}