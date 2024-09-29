using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BlueSurpriseBagHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> blueSurpriseBags;

        public BlueSurpriseBagHandler()
        {
            blueSurpriseBags = Context.Server.Values.GetUInt16HashSet("values.items.blueSurpriseBags");
        }

        private static List< (ushort OpenTibiaId, byte Count) > prizes = new List< (ushort OpenTibiaId, byte Count) >()
        {
            (6575, 1),
            (6577, 1),
            (2687, 10),
            (6394, 1),
            (6572, 1),
            (6574, 1),
            (6569, 3),
            (6578, 1),
            (6280, 1),
            (6576, 1),
            (2114, 1)
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (blueSurpriseBags.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.GiftWraps) ).Then( () =>
                {
                    int value = Context.Server.Randomization.Take(0, prizes.Count - 1);

                    return Context.AddCommand(new ItemTransformCommand(command.Item, prizes[value].OpenTibiaId, prizes[value].Count) );
                } );
            }

            return next();
        }
    }
}