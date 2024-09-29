using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class KeyHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> keys;
        private readonly Dictionary<ushort, ushort> unlockedDoors;
        private readonly Dictionary<ushort, ushort> lockedDoors;

        public KeyHandler()
        {
            keys = Context.Server.Values.GetUInt16HashSet("values.items.keys");
            unlockedDoors = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.unlockedDoors");
            lockedDoors = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.lockedDoors");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (keys.Contains(command.Item.Metadata.OpenTibiaId) && command.Item.ActionId == command.ToItem.ActionId)
            {
                if (unlockedDoors.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                }
                else if (lockedDoors.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                }
            }

            return next();
        }
    }
}