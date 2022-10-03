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

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>()
        {
            { 469, 468 },
            { 482, 481 },
            { 484, 483 }
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
            context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) ).Then( (ctx, item) =>
            {
                return ctx.AddCommand(new ItemDecayCommand(item, 10000, decay[item.Metadata.OpenTibiaId], 1) );

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}