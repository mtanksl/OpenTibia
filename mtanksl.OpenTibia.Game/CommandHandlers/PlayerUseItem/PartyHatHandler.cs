using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PartyHatHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> partyHats;

        public PartyHatHandler()
        {
            partyHats = Context.Server.Values.GetUInt16HashSet("values.items.partyHats");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (partyHats.Contains(command.Item.Metadata.OpenTibiaId) && command.Item.Parent is Inventory inventory && (Slot)inventory.GetIndex(command.Item) == Slot.Head)
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.PartyAnimal, 200, "Party Animal") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.GiftWraps) );
                } );
            }

            return next();
        }
    }
}