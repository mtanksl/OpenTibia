using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfCakeDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> lumpOfCakeDoughs = new HashSet<ushort>() { 6277 };

        private static HashSet<ushort> ovens = new HashSet<ushort>() { 1786, 1788, 1790, 1792, 6356, 6358, 6360, 6362 };

        private static ushort cake = 6278;

        private static HashSet<ushort> barOfChocolate = new HashSet<ushort>() { 6574 };

        private static ushort lumpOfChocolateDough = 8846;

        private static HashSet<ushort> bakingTrays = new HashSet<ushort>() { 2561 };

        private static ushort bakingTrayWithDough = 8848;

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfCakeDoughs.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.PieceOfCakeDecoratedCake, new[] { AchievementConstants.PieceOfCakeChocolateCake, AchievementConstants.PieceOfCakeDecoratedCake }, "Piece of Cake") ).Then( () =>
                    {
                        return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, cake, 1) );
                    } );
                }
                else if (barOfChocolate.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.ToItem, lumpOfChocolateDough, 1) );
                    } );
                }
                else if (bakingTrays.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.ToItem, bakingTrayWithDough, 1) );
                    } );
                }   
            }

            return next();
        }
    }
}