using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FlourHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> flours = new HashSet<ushort>() { 2692 };

        private HashSet<ushort> buckets = new HashSet<ushort>() { 1775, 2005 };

        private ushort lumpOfDough = 2693;

        private ushort lumpOfCakeDough = 6277;

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (flours.Contains(command.Item.Metadata.OpenTibiaId) && buckets.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            FluidItem toFluidItem = (FluidItem)command.ToItem;

            if (toFluidItem.FluidType == FluidType.Water)
            {
                if (command.Item is StackableItem stackableItem && stackableItem.Count > 1)
                {
                    context.AddCommand(new ItemUpdateCommand(stackableItem, (byte)(stackableItem.Count - 1) ) );
                }
                else
                {
                    context.AddCommand(new ItemDestroyCommand(command.Item) );
                }

                context.AddCommand(new ItemUpdateCommand(toFluidItem, (byte)FluidType.Empty) );

                context.AddCommand(new ItemCreateCommand(command.Player.Tile, lumpOfDough, 1) );
            }
            else if (toFluidItem.FluidType == FluidType.Milk)
            {
                if (command.Item is StackableItem stackableItem && stackableItem.Count > 1)
                {
                    context.AddCommand(new ItemUpdateCommand(stackableItem, (byte)(stackableItem.Count - 1) ) );
                }
                else
                {
                    context.AddCommand(new ItemDestroyCommand(command.Item) );
                }

                context.AddCommand(new ItemUpdateCommand(toFluidItem, (byte)FluidType.Empty) );

                context.AddCommand(new ItemCreateCommand(command.Player.Tile, lumpOfCakeDough, 1) );
            }

            base.Handle(context, command);
        }
    }
}