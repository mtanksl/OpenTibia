using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TwinSunHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> charms = new HashSet<ushort>() { 11261 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (charms.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerBlessCommand(command.Player, "The Fire of the Suns engulfs you.", "Twin Sun Charm") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.RedShimmer) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                } );
            }

            return next();
        }
    }
}