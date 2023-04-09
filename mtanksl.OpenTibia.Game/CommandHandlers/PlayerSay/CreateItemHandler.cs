using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateItemHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(ContextPromiseDelegate next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/i") )
            {
                int startIndex = command.Message.IndexOf(' ');

                if (startIndex != -1)
                {
                    ushort toOpenTibiaId;

                    if (ushort.TryParse(command.Message.Substring(startIndex + 1), out toOpenTibiaId) )
                    {
                        Tile toTile = context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                        if (toTile != null)
                        {
                            return context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) ).Then(ctx =>
                            {
                                return ctx.AddCommand(new TileIncrementOrCreateItemCommand(toTile, toOpenTibiaId, 1) );
                            } );
                        }

                        return context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
                    }
                }
            }

            return next(context);
        }
    }
}