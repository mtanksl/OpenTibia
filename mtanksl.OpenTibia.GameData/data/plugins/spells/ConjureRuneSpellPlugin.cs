using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class ConjureRuneSpellPlugin : SpellPlugin
    {
        private static ushort blankRune = 2260;

        public ConjureRuneSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Context.AddCommand(new PlayerCountItemsCommand(player, blankRune, 1) ).Then( (count) =>
            {
                return count > 0 ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
            } );
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            ushort openTibiaId = Spell.ConjureOpenTibiaId.Value;

            int count = Spell.ConjureCount.Value;

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.AddCommand(new PlayerCreateItemsCommand(player, openTibiaId, 1, count) );

            } ).Then( () =>
            {
                return Context.AddCommand(new PlayerDestroyItemsCommand(player, blankRune, 1, 1) );
            } );
        }
     }
}