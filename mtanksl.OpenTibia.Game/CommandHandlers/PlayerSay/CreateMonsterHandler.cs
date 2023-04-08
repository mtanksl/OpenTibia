using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateMonsterHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/m") )
            {
                int startIndex = command.Message.IndexOf(' ');

                if (startIndex != -1)
                {
                    string name = command.Message.Substring(startIndex + 1);

                    Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                    if (toTile != null)
                    {
                        return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) ).Then(ctx =>
                        {
                            return ctx.AddCommand(new TileCreateMonsterCommand(toTile, name) );
                        } );
                    }

                    return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
                }
            }

            return next(context);
        }
    }
}