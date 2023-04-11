using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SurpriseBagBlueHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> surpriseBags = new HashSet<ushort>() { 6570 };

        private List< (ushort OpenTibiaId, byte Count) > prizes = new List< (ushort OpenTibiaId, byte Count) >()
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
            if (surpriseBags.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(0, prizes.Count);

                switch (command.Item.Root() )
                {
                    case Tile tile:

                        Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.GiftWraps) );

                        break;

                    case Inventory inventory:

                        Context.AddCommand(new ShowMagicEffectCommand(inventory.Player.Tile.Position, MagicEffectType.GiftWraps) );

                        break;
                }
                               
                Context.AddCommand(new ItemTransformCommand(command.Item, prizes[value].OpenTibiaId, prizes[value].Count) );

                return Promise.Completed;
            }

            return next();
        }
    }
}