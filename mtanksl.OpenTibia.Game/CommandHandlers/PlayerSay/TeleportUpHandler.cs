using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportUpHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Context context, Func<Context, Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/up") )
            {
                Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(0, 0, -1) );

                if (toTile != null)
                {
                    return context.AddCommand(new CreatureUpdateParentCommand(command.Player, toTile) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                    } );
                }
                else
                {
                    return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
                } 
            }

            return next(context);
        }
    }
}