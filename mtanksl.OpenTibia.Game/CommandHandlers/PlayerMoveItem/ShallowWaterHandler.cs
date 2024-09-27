using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ShallowWaterHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly HashSet<ushort> shallowWaters;

        public ShallowWaterHandler()
        {
            shallowWaters = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.shallowWaters") );
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && shallowWaters.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueRings) ).Then( () =>
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, command.Count) );
                } );
            }

            return next();
        }
    }
}