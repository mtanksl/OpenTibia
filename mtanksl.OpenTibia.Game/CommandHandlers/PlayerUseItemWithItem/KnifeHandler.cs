using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class KnifeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> knifes = new HashSet<ushort>() { 2566 };

        private HashSet<ushort> pumpkins = new HashSet<ushort>() { 2683 };

        private ushort pumpkinhead = 2096;

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemWithItemCommand command)
        {
            if (knifes.Contains(command.Item.Metadata.OpenTibiaId) && pumpkins.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return context.AddCommand(new ItemTransformCommand(command.ToItem, pumpkinhead, 1) );
            }

            return next(context);
        }
    }
}