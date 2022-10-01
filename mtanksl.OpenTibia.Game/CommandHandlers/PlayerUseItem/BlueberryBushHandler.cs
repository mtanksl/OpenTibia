using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BlueberryBushHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> blueberryBushs = new Dictionary<ushort, ushort>() 
        {
            { 2785, 2786 }
        };

        private ushort blueberry = 2677;

        private ushort toOpenTibiaId;

        public override bool CanHandle(Context context, PlayerUseItemCommand command)
        {
            if (blueberryBushs.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemCommand command)
        {
            context.AddCommand(new ItemCreateCommand( (Tile)command.Item.Parent, blueberry, 3) ).Then( (ctx, item) =>
            {
                return ctx.AddCommand(new ItemReplaceCommand(command.Item, toOpenTibiaId, 1) );
            
            } ).Then( (ctx, item) =>
            {
                OnComplete(ctx);
            } );
        }
    }
}