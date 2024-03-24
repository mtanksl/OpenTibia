using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreateItemHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/i ") && command.Player.Rank == Rank.Gamemaster)
            {
                string[] split = command.Message.Split(" ");

                if (split.Length == 2)
                {
                    ushort toOpenTibiaId;

                    if (ushort.TryParse(split[1], out toOpenTibiaId) )
                    {
                        Tile toTile = Context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                        if (toTile != null)
                        {
                            return Context.AddCommand(new TileCreateItemOrIncrementCommand(toTile, toOpenTibiaId, 1) ).Then( () =>
                            {
                                return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );
                            } );
                        }

                        return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
                    }
                }
                else 
                {
                    ushort toOpenTibiaId;

                    byte count;

                    if (ushort.TryParse(split[1], out toOpenTibiaId) && byte.TryParse(split[2], out count) && count >= 1 && count <= 100)
                    {
                        Tile toTile = Context.Server.Map.GetTile(command.Player.Tile.Position.Offset(command.Player.Direction) );

                        if (toTile != null)
                        {
                            return Context.AddCommand(new TileCreateItemOrIncrementCommand(toTile, toOpenTibiaId, count) ).Then( () =>
                            {
                                return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) );
                            } );
                        }

                        return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.Puff) );
                    }
                }
            }

            return next();
        }
    }
}