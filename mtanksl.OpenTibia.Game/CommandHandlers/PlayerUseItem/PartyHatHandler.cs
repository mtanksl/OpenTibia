using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PartyHatHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> partyHats = new HashSet<ushort>() { 6578 };

        public override Promise Handle(ContextPromiseDelegate next, PlayerUseItemCommand command)
        {
            if (partyHats.Contains(command.Item.Metadata.OpenTibiaId) && command.Item.Parent is Inventory)
            {
                context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.GiftWraps) );

                return Promise.Completed(context);
            }

            return next(context);
        }
    }
}