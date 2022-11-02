using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class KickPlayerHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/kick") )
            {
                int startIndex = command.Message.IndexOf(' ');

                if (startIndex != -1)
                {
                    string name = command.Message.Substring(startIndex + 1);

                    Player player = context.Server.GameObjects.GetPlayers()
                        .Where(p => p.Name == name)
                        .FirstOrDefault();

                    if (player != null && player != command.Player)
                    {
                        return context.AddCommand(new ParseLogOutCommand(player) );
                    }

                    return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
                }
            }

            return next(context);
        }
    }
}