using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LumpOfChocolateDoughHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> lumpOfChocolateDoughs;
        private readonly HashSet<ushort> ovens;
        private readonly ushort chocolateCake;

        public LumpOfChocolateDoughHandler()
        {
            lumpOfChocolateDoughs = Context.Server.Values.GetUInt16HashSet("values.items.lumpOfChocolateDoughs");
            ovens = Context.Server.Values.GetUInt16HashSet("values.items.ovens");
            chocolateCake = Context.Server.Values.GetUInt16("values.items.chocolateCake");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (lumpOfChocolateDoughs.Contains(command.Item.Metadata.OpenTibiaId) && ovens.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.PieceOfCakeChocolateCake, new[] { AchievementConstants.PieceOfCakeChocolateCake, AchievementConstants.PieceOfCakeDecoratedCake }, "Piece of Cake") ).Then( () =>
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, chocolateCake, 1) );
                } );
            }

            return next();
        }
    }
}