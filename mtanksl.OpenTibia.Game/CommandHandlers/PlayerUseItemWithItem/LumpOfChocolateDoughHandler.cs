using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfChocolateDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> lumpOfChocolateDough = new HashSet<ushort>() { 8846 };

        private HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private ushort chocolateCake = 8847;

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfChocolateDough.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (command.Item is StackableItem stackableItem && stackableItem.Count > 1)
            {
                context.AddCommand(new ItemUpdateCommand(stackableItem, (byte)(stackableItem.Count - 1) ) );
            }
            else
            {
                context.AddCommand(new ItemDestroyCommand(command.Item) );
            }

            context.AddCommand(new ItemCreateCommand( (Tile)command.ToItem.Parent, chocolateCake, 1) );

            base.Handle(context, command);
        }
    }
}