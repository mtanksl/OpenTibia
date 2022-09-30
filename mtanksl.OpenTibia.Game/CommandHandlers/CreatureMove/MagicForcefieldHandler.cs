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
            if (command.ToTile.TopItem != null && magicForcefields.Contains(command.ToTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, CreatureMoveCommand command)
        {
            Tile toOtherTile = context.Server.Map.GetTile( ( (TeleportItem)command.ToTile.TopItem ).Position );

            context.AddCommand(new CreatureMoveCommand(command.Creature, toOtherTile), ctx =>
            {
                ctx.AddCommand(new MagicEffectCommand(command.ToTile.Position, MagicEffectType.Teleport) );

                ctx.AddCommand(new MagicEffectCommand(toOtherTile.Position, MagicEffectType.Teleport) );

                OnComplete(ctx);
            } );
        }
    }
}