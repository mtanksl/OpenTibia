using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MacheteHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> machetes = new HashSet<ushort>() { 2420 };

        private Dictionary<ushort, ushort> jungleGrass = new Dictionary<ushort, ushort>()
        {
            { 2782, 2781 }
        };

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if ( machetes.Contains(command.Item.Metadata.OpenTibiaId) && jungleGrass.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
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