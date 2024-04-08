using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MagicForcefield2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private static HashSet<ushort> magicForcefields = new HashSet<ushort>() { 1387 };

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile tile)
            {
                Tile magicForcefield = tile;

                if (magicForcefield.TopItem != null && magicForcefields.Contains(magicForcefield.TopItem.Metadata.OpenTibiaId) )
                {
                    Tile toTile = Context.Server.Map.GetTile( ( (TeleportItem)magicForcefield.TopItem ).Position );

                    if (toTile != null)
                    {
                        return Context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 255, command.Count, false) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(magicForcefield.Position, MagicEffectType.Teleport) );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                        } );
                    }
                }
            }

            return next();
        }
    }
}