using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PiggyBankHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> piggyBanks = new Dictionary<ushort, ushort>()
        {
            { 2114, 2115 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (piggyBanks.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                switch (command.Item.Root() )
                {
                    case Tile tile:

                        Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.Puff) );

                        break;

                    case Inventory inventory:

                        Context.AddCommand(new ShowMagicEffectCommand(inventory.Player.Tile.Position, MagicEffectType.Puff) );

                        break;
                }

                Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );

                return Promise.Completed();
            }

            return next();
        }
    }
}