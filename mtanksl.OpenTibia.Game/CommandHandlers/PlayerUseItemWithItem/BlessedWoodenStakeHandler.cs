using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BlessedWoodenStakeHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> blessedWoodenStakes;
        private readonly Dictionary<ushort, ushort> dusts;

        public BlessedWoodenStakeHandler()
        {
            blessedWoodenStakes = Context.Server.Values.GetUInt16HashSet("values.items.blessedWoodenStakes");
            dusts = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.dusts");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (blessedWoodenStakes.Contains(command.Item.Metadata.OpenTibiaId) && dusts.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                //TODO
            }

            return next();
        }
    }
}