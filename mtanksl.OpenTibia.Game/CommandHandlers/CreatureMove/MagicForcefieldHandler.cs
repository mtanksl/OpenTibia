using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MagicForcefieldHandler : CommandHandler<CreatureMoveCommand>
    {
        private HashSet<ushort> magicForcefields = new HashSet<ushort>() { 1387 };

        public override bool CanHandle(Context context, CreatureMoveCommand command)
        {
            Tile toTile = command.ToTile;

            if (toTile.TopItem != null && magicForcefields.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, CreatureMoveCommand command)
        {
            Tile toTile = command.ToTile;

            Tile toOtherTile = context.Server.Map.GetTile( ( (TeleportItem)toTile.TopItem ).Position );

            context.AddCommand(new CreatureMoveCommand(command.Creature, toOtherTile) );

            context.AddCommand(new MagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

            context.AddCommand(new MagicEffectCommand(toOtherTile.Position, MagicEffectType.Teleport) );

            base.Handle(context, command);
        }
    }
}