using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MagicForcefieldHandler : CommandHandler<CreatureMoveCommand>
    {
        private readonly HashSet<ushort> magicForcefields;

        public MagicForcefieldHandler()
        {
            magicForcefields = Context.Server.Values.GetUInt16HashSet("values.items.magicForcefields");
        }

        public override Promise Handle(Func<Promise> next, CreatureMoveCommand command)
        {
            Tile magicForcefield = command.ToTile;

            if (magicForcefield.TopItem != null && magicForcefields.Contains(magicForcefield.TopItem.Metadata.OpenTibiaId) )
            {
                Tile toTile = Context.Server.Map.GetTile( ( (TeleportItem)magicForcefield.TopItem ).Position );

                if (toTile != null)
                {
                    return Context.AddCommand(new CreatureMoveCommand(command.Creature, toTile) ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(magicForcefield.Position, MagicEffectType.Teleport) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
                    } );
                }
            }

            return next();
        }
    }
}