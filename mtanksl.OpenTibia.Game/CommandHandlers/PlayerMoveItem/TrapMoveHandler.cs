using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapMoveHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly Dictionary<ushort, ushort> traps;

        public TrapMoveHandler()
        {
            traps = LuaScope.GetInt16Int16Dictionary(Context.Server.Values.GetValue("values.items.decay.traps") );
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Tile tile)
            {
                ushort toOpenTibiaId;

                if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new TileCreateItemOrIncrementCommand(tile, toOpenTibiaId, 1) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemDestroyCommand(command.Item) );            
                    } );
                }
            }

            return next();
        }
    }
}