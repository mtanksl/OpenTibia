using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PartyHatHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> partyHats = new HashSet<ushort>() { 6578 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (partyHats.Contains(command.Item.Metadata.OpenTibiaId) && command.Item.Parent is Inventory inventory && (Slot)inventory.GetIndex(command.Item) == Slot.Head)
            {
                int count;

                command.Player.Client.Storages.TryGetValue(AchievementConstants.PartyAnimal, out count);

                command.Player.Client.Storages.SetValue(AchievementConstants.PartyAnimal, ++count);

                if (count >= 200)
                {
                    if ( !command.Player.Client.Achievements.HasAchievement("Party Animal") )
                    {
                        command.Player.Client.Achievements.SetAchievement("Party Animal");

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"Party Animal\".") );
                    }
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.GiftWraps) );
            }

            return next();
        }
    }
}