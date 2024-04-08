using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SurpriseBagSuspiciousHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> surpriseBags = new HashSet<ushort>() { 9108 };

        private static List< (ushort OpenTibiaId, byte Count) > prizes = new List< (ushort OpenTibiaId, byte Count) >()
        {
            (2148, 50),
            (2667, 1),
            (2114, 1),
            (1689, 1),
            (7735, 1),
            (8110, 1),
            (6394, 1),
            (7377, 1),
            (6574, 1),
            (7487, 1),
            (9693, 1)
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (surpriseBags.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(1, 5);

                if (value == 1)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.GiftWraps) ).Then( () =>
                    {
                        value = Context.Server.Randomization.Take(0, prizes.Count - 1);

                        return Context.AddCommand(new ItemTransformCommand(command.Item, prizes[value].OpenTibiaId, prizes[value].Count) );
                    } );
                }
                else
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                    } );
                }
            }

            return next();
        }
    }
}