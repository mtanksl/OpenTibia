using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class HealthPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private HashSet<ushort> healthPotions = new HashSet<ushort>() { 7618 };

        public override bool CanHandle(Context context, PlayerUseItemWithCreatureCommand command)
        {
            if (healthPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player)
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithCreatureCommand command)
        {
            if (command.Item is StackableItem stackableItem && stackableItem.Count > 1)
            {
                context.AddCommand(new ItemUpdateCommand(stackableItem, (byte)(stackableItem.Count - 1) ) );
            }
            else
            {
                context.AddCommand(new ItemDestroyCommand(command.Item) );
            }

            context.AddCommand(new ShowTextCommand(command.ToCreature, TalkType.MonsterSay, "Aaaah...") );

            context.AddCommand(new ShowMagicEffectCommand(command.ToCreature.Tile.Position, MagicEffectType.RedShimmer) );

            OnComplete(context);
        }
    }
}