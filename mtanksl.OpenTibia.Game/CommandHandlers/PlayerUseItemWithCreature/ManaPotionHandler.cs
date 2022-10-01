using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ManaPotionHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private HashSet<ushort> manaPotions = new HashSet<ushort>() { 7620 };

        public override bool CanHandle(Context context, PlayerUseItemWithCreatureCommand command)
        {
            if (manaPotions.Contains(command.Item.Metadata.OpenTibiaId) && command.ToCreature is Player)
            {
                return true;
            }

            return false;
        }

        public override void Handle(Context context, PlayerUseItemWithCreatureCommand command)
        {
            context.AddCommand(new ItemDecrementCountCommand(command.Item, 1) ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowTextCommand(command.ToCreature, TalkType.MonsterSay, "Aaaah...") );

            } ).Then(ctx =>
            {
                return ctx.AddCommand(new ShowMagicEffectCommand(command.ToCreature.Tile.Position, MagicEffectType.BlueShimmer) );

            } ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}