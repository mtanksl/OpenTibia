using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class WaterBallsHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly HashSet<ushort> shallowWaters;
        private readonly HashSet<ushort> waterBalls;
        private readonly Dictionary<ushort, ushort> waterBallsLandToWater;
        private readonly Dictionary<ushort, ushort> waterBallsWaterToLand;

        public WaterBallsHandler()
        {
            shallowWaters = Context.Server.Values.GetUInt16HashSet("values.items.shallowWaters");
            waterBalls = Context.Server.Values.GetUInt16HashSet("values.items.waterBalls");
            waterBallsLandToWater = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.waterBallsLandToWater");
            waterBallsWaterToLand = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.waterBallsWaterToLand");
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (waterBalls.Contains(command.Item.Metadata.OpenTibiaId) && command.ToContainer is Tile toTile && toTile.Ground != null)
            {
                if (shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
                {
                    ushort toOpenTibiaId;

                    if (waterBallsLandToWater.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.WaterSplash) ).Then( () =>
                        {
                            return Context.AddCommand(new TileCreateItemCommand(toTile, toOpenTibiaId, 1) );

                        } ).Then( (item) =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                        } );
                    }
                    else
                    {
                        return command.Execute();
                    }
                }
                else
                {
                    ushort toOpenTibiaId;

                    if (waterBallsWaterToLand.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
                    {
                        return Context.AddCommand(new TileCreateItemCommand(toTile, toOpenTibiaId, 1) ).Then( (item) =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                        } );
                    }
                }
            }

            return next();
        }
    }
}