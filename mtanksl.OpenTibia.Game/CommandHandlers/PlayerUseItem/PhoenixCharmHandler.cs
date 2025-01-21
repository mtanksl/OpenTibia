using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PhoenixCharmHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> phoenixCharms;
        
        public PhoenixCharmHandler()
        {
            phoenixCharms = Context.Server.Values.GetUInt16HashSet("values.items.phoenixCharms");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (phoenixCharms.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerBlessCommand(command.Player, "The Spark of the Phoenix emblazes you.", "Phoenix Charm") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.FireDamage) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                } );
            }

            return next();
        }
    }
}