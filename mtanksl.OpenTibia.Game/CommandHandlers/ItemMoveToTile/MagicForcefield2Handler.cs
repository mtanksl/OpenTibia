using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MagicForcefield2Handler : CommandHandler<ItemMoveToTileCommand>
    {
        private HashSet<ushort> magicForcefields = new HashSet<ushort>() { 1387 };

        public override bool CanHandle(Context context, ItemMoveToTileCommand command)
        {
            if (command.ToTile.TopItem != null && magicForcefields.Contains(command.ToTile.TopItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, ItemMoveToTileCommand command)
        {
            Tile fromTile = command.ToTile;

            Tile toTile = context.Server.Map.GetTile( ( (TeleportItem)fromTile.TopItem ).Position );

            context.AddCommand(new ItemMoveToTileCommand(command.Item, toTile) ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand(fromTile.Position, MagicEffectType.Teleport) );

            } ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}