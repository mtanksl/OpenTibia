using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class KnifeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> knifes = new HashSet<ushort>() { 2566, 10515, 10511 };

        private HashSet<ushort> pumpkins = new HashSet<ushort>() { 2683 };

        private ushort pumpkinhead = 2096;

        private HashSet<ushort> fruits = new HashSet<ushort>() { 2676, 2677, 2684, 2679, 2678, 2681, 8841, 5097, 2672, 2675, 2673, 8839, 8840, 2674, 2680 };

        private ushort cake = 6278;

        private ushort decoratedCake = 6279;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (knifes.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (pumpkins.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, pumpkinhead, 1) );
                }
                else if (fruits.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    Item item = command.Player.Inventory.GetContent( (byte)Slot.Extra) as Item;

                    if (item != null)
                    {
                        if (item.Metadata.OpenTibiaId == cake)
                        {
                            return Context.AddCommand(new ItemDecrementCommand(command.ToItem, 1) ).Then( () =>
                            {
                                return Context.AddCommand(new ItemTransformCommand(item, decoratedCake, 1) );
                            } );
                        }
                    }
                }
            }

            return next();
        }
    }
}