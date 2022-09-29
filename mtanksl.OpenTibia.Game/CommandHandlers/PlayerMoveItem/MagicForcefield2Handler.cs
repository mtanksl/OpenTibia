using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MagicForcefield2Handler : CommandHandler<PlayerMoveItemCommand>
    {
        private HashSet<ushort> magicForcefields = new HashSet<ushort>() { 1387 };

        public override bool CanHandle(Context context, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.TopItem != null && magicForcefields.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerMoveItemCommand command)
        {
            Tile fromTile = (Tile)command.ToContainer;

            Tile toTile = context.Server.Map.GetTile( ( (TeleportItem)fromTile.TopItem ).Position );

            context.AddCommand(new PlayerMoveItemCommand(command.Player, command.Item, toTile, 0, command.Count) );

            context.AddCommand(new MagicEffectCommand(fromTile.Position, MagicEffectType.Teleport) );

            context.AddCommand(new MagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

            base.Handle(context, command);
        }
    }
}