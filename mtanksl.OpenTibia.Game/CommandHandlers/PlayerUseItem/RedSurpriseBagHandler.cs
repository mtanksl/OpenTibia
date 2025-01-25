using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RedSurpriseBagHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> redSurpriseBags;

        public RedSurpriseBagHandler()
        {
            redSurpriseBags = Context.Server.Values.GetUInt16HashSet("values.items.redSurpriseBags");
        }

        private static List< (ushort OpenTibiaId, byte Count) > prizes = new List< (ushort OpenTibiaId, byte Count) >()
        {
            (6574, 1),
            (2195, 1),
            (6394, 1),
            (6576, 1),
            (6578, 1),
            (2114, 1),
            (5944, 1),
            (2153, 1),
            (2520, 1),
            (2492, 1),
            (5080, 1),
            (2156, 1),
            (2498, 1),
            (2112, 1),
            (2173, 1),
            (5791, 1)
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (redSurpriseBags.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.GiftWraps) ).Then( () =>
                {
                    (ushort openTibiaId, byte count) = Context.Server.Randomization.Take(prizes);

                    return Context.AddCommand(new ItemTransformCommand(command.Item, openTibiaId, count) );
                } );
            }

            return next();
        }
    }
}