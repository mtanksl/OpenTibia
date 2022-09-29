using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class KnifeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> knifes = new HashSet<ushort>() { 2566 };

        private HashSet<ushort> pumpkins = new HashSet<ushort>() { 2683 };

        private ushort pumpkinhead = 2096;

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (knifes.Contains(command.Item.Metadata.OpenTibiaId) && pumpkins.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            context.AddCommand(new ItemReplaceCommand(command.ToItem, pumpkinhead, 1) );

            base.Handle(context, command);
        }
    }
}