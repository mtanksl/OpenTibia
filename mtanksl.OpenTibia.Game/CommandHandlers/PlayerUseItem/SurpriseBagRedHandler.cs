using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SurpriseBagRedHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> surpriseBags = new HashSet<ushort>() { 6571 };

        private List< (ushort OpenTibiaId, byte Count) > prizes = new List< (ushort OpenTibiaId, byte Count) >()
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
            if (surpriseBags.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(0, prizes.Count - 1);

                switch (command.Item.Root() )
                {
                    case Tile tile:

                        return Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.GiftWraps) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.Item, prizes[value].OpenTibiaId, prizes[value].Count) );
                        } );

                    case Inventory inventory:
                    case null:

                        return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.GiftWraps) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemTransformCommand(command.Item, prizes[value].OpenTibiaId, prizes[value].Count) );
                        } );
                }
            }

            return next();
        }
    }
}