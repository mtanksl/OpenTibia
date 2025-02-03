using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SawHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> saws;
        private readonly HashSet<ushort> woodens;
        private readonly ushort woodenTies;

        public SawHandler()
        {
            saws = Context.Server.Values.GetUInt16HashSet("values.items.saws");
            woodens = Context.Server.Values.GetUInt16HashSet("values.items.woodens");
            woodenTies = Context.Server.Values.GetUInt16("values.items.woodenTies");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (saws.Contains(command.Item.Metadata.OpenTibiaId) && woodens.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.Puff) ).Then( () =>
                {
                    return Context.AddCommand(new ItemDecrementCommand(command.ToItem, 1) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new PlayerCreateItemCommand(command.Player, woodenTies, 1) );
                } );
            }

            return next();
        }
    }
}