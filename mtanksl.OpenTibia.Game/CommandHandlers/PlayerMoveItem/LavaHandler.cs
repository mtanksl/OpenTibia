using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LavaHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly HashSet<ushort> lavas;

        public LavaHandler()
        {
            lavas = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.lavas") );
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.Ground != null && lavas.Contains(toTile.Ground.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.FirePlume) ).Then( () =>
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.Item, command.Count) );
                } );
            }

            return next();
        }
    }
}