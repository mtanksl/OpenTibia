using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class DustbinHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly HashSet<ushort> dustbins;

        public DustbinHandler()
        {
            dustbins = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.dustbins") );
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile toTile && toTile.TopItem != null && dustbins.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ItemDecrementCommand(command.Item, command.Count) );
            }

            return next();
        }
    }
}