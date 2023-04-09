using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportDownHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(ContextPromiseDelegate next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/down") )
            {
                Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(0, 0, 1) );

                if (toTile != null)
                {
                    return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new CreatureUpdateParentCommand(command.Player, toTile) );
                    } );
                }

                return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
            }

            return next(context);
        }
    }
}