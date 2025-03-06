using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ScytheHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> scythes;
        private readonly Dictionary<ushort, ushort> wheats;
        private readonly ushort wheat;

        public ScytheHandler()
        {
            scythes = Context.Server.Values.GetUInt16HashSet("values.items.scythes");
            wheats = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.wheats");
            wheat = Context.Server.Values.GetUInt16("values.items.wheat");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (scythes.Contains(command.Item.Metadata.OpenTibiaId) && wheats.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.HappyFarmer, 200, "Happy Farmer") ).Then( () =>
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand( (Tile)command.ToItem.Parent, wheat, 1) );
                
                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                } );
            }
                
            return next();
        }
    }
}