using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TeleportHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Context context, ContextPromiseDelegate next, PlayerSayCommand command)
        {
            int count;

            if (command.Message.StartsWith("/a") && command.Message.Contains(" ") && int.TryParse(command.Message.Split(' ')[1], out count) && count > 0)
            {
                Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction, count) );

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