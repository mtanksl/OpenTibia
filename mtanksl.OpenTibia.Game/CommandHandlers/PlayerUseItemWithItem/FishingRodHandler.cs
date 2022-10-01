using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FishingRodHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> fishingRods = new HashSet<ushort>() { 2580 };

        private HashSet<ushort> shallowWaters = new HashSet<ushort> { 4608, 4609, 4610, 4611, 4612, 4613, 4612, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4664, 4665, 4666 };

        public override bool CanHandle(Context context, PlayerUseItemWithItemCommand command)
        {
            if (fishingRods.Contains(command.Item.Metadata.OpenTibiaId) && shallowWaters.Contains(command.ToItem.Metadata.OpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithItemCommand command)
        {
            context.AddCommand(new ShowMagicEffectCommand( ( (Tile)command.ToItem.Parent).Position, MagicEffectType.BlueRings) ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}