using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;

namespace OpenTibia.GameData.Plugins.Runes
{
    public class CurePoisonRunePlugin : RunePlugin
    {
        public CurePoisonRunePlugin(Rune rune) : base(rune)
        {

        }

        public override void Start()
        {
            
        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            return Promise.FromResult(true);
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureRemoveConditionCommand(target, ConditionSpecialCondition.Poisoned) );
            } );
        }

        public override void Stop()
        {
            
        }
    }
}
