using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TwinSunCharmHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> twinSunCharms;

        public TwinSunCharmHandler()
        {
            twinSunCharms = Context.Server.Values.GetUInt16HashSet("values.items.twinSunCharms");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (twinSunCharms.Contains(command.Item.Metadata.OpenTibiaId) )
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