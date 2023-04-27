using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class KeyHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> keys = new HashSet<ushort>() { 2086, 2087, 2088, 2089, 2090, 2091, 2092 };

        private Dictionary<ushort, ushort> unlockDoors = new Dictionary<ushort, ushort>()
        {
            { 1209, 1211 },
            { 1212, 1214 },
            { 1231, 1233 },
            { 1234, 1236 },
            { 1249, 1251 },
            { 1252, 1254 },
            { 3535, 3537 },
            { 3544, 3546 },

            //TODO: More items
        };

        private Dictionary<ushort, ushort> lockDoors = new Dictionary<ushort, ushort>()
        {
            { 1211, 1209 },
            { 1214, 1212 },
            { 1233, 1231 },
            { 1236, 1234 },
            { 1251, 1249 },
            { 1254, 1252 },
            { 3537, 3535 },
            { 3546, 3544 },

            //TODO: More items
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (keys.Contains(command.Item.Metadata.OpenTibiaId) && command.Item.ActionId == command.ToItem.ActionId)
            {
                if (unlockDoors.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                }
                else if (lockDoors.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );
                }
            }

            return next();
        }
    }
}