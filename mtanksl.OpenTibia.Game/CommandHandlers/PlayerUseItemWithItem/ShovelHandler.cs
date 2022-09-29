using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ShovelHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> shovels = new HashSet<ushort>() { 2554, 5710 };

        private Dictionary<ushort, ushort> stonePiles = new Dictionary<ushort, ushort>()
        {
            { 468, 469 },
            { 481, 482 },
            { 483, 484 }
        };

        ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (shovels.Contains(command.Item.Metadata.OpenTibiaId) && stonePiles.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            context.AddCommand(new ItemReplaceCommand(command.ToItem, toOpenTibiaId, 1) );

            base.Handle(context, command);
        }
    }
}