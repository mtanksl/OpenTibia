using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfCakeDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> lumpOfCakeDoughs;
        private readonly HashSet<ushort> ovens;
        private readonly ushort cake;
        private readonly HashSet<ushort> barOfChocolates;
        private readonly ushort lumpOfChocolateDough;
        private readonly HashSet<ushort> bakingTrays;
        private readonly ushort bakingTrayWithDough;

        public LumpOfCakeDoughHandler()
        {
            lumpOfCakeDoughs = Context.Server.Values.GetUInt16HashSet("values.items.lumpOfCakeDoughs");
            ovens = Context.Server.Values.GetUInt16HashSet("values.items.ovens");
            cake = Context.Server.Values.GetUInt16("values.items.cake");
            barOfChocolates = Context.Server.Values.GetUInt16HashSet("values.items.barOfChocolates");
            lumpOfChocolateDough = Context.Server.Values.GetUInt16("values.items.lumpOfChocolateDough");
            bakingTrays = Context.Server.Values.GetUInt16HashSet("values.items.bakingTrays");
            bakingTrayWithDough = Context.Server.Values.GetUInt16("values.items.bakingTrayWithDough");
        }

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
                else if (barOfChocolates.Contains(command.ToItem.Metadata.OpenTibiaId) )
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