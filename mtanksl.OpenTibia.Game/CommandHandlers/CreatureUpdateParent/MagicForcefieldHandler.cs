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

        public override Promise Handle(Context context, Func<Context, Promise> next, CreatureUpdateParentCommand command)
        {
            Tile toTile = command.ToTile;

            if (toTile.TopItem != null && magicForcefields.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                Tile toOtherTile = context.Server.Map.GetTile( ( (TeleportItem)toTile.TopItem ).Position );

                return context.AddCommand(new CreatureUpdateParentCommand(command.Creature, toOtherTile) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

                } ).Then(ctx =>
                {
                    return ctx.AddCommand(new ShowMagicEffectCommand(toOtherTile.Position, MagicEffectType.Teleport) );
                } );
            }

            return next(context);
        }
    }
}