using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MagicForcefieldHandler : CommandHandler<CreatureUpdateParentCommand>
    {
        private HashSet<ushort> magicForcefields = new HashSet<ushort>() { 1387 };

        public override Promise Handle(Context context, ContextPromiseDelegate next, CreatureUpdateParentCommand command)
        {
            Tile magicForcefield = command.ToTile;

            if (magicForcefield.TopItem != null && magicForcefields.Contains(magicForcefield.TopItem.Metadata.OpenTibiaId) )
            {
                Tile toTile = context.Server.Map.GetTile( ( (TeleportItem)magicForcefield.TopItem ).Position );

                if (toTile != null)
                {
                    return context.AddCommand(new CreatureUpdateParentCommand(command.Creature, toTile) ).Then(ctx =>
                    {
                        return ctx.AddCommand(new ShowMagicEffectCommand(magicForcefield.Position, MagicEffectType.Teleport) );

                    } ).Then(ctx =>
                    {
                        return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                    } );
                }
            }

            return next(context);
        }
    }
}