using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfCakeDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> lumpOfCakeDoughs = new HashSet<ushort>() { 6277 };

        private HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private ushort cake = 6278;

        private HashSet<ushort> barOfChocolate = new HashSet<ushort>() { 6574 };

        private ushort lumpOfChocolateDough = 8846;

        private HashSet<ushort> bakingTrays = new HashSet<ushort>() { 2561 };

        private ushort bakingTrayWithDough = 8848;

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfCakeDoughs.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return true;
                }
                else if (barOfChocolate.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return true;
                }
                else if (bakingTrays.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return true;
                }
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                context.AddCommand(new ItemDecrementCountCommand( (StackableItem)command.Item, 1) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemCreateCommand( (Tile)command.ToItem.Parent, cake, 1) );

                } ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );                
            }
            else if (barOfChocolate.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                context.AddCommand(new ItemDecrementCountCommand( (StackableItem)command.Item, 1) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemReplaceCommand(command.ToItem, lumpOfChocolateDough, 1) );

                } ).Then( (ctx, item) =>
                {
                    OnComplete(ctx);
                } );
            }
            else if (bakingTrays.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                context.AddCommand(new ItemDecrementCountCommand( (StackableItem)command.Item, 1) ).Then(ctx =>
                {
                    return ctx.AddCommand(new ItemReplaceCommand(command.ToItem, bakingTrayWithDough, 1) );

                } ).Then( (ctx, item) =>
                {
                    OnComplete(ctx);
                } );
            }            
        }
    }
}