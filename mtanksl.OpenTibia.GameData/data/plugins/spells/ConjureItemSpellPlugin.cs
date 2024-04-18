using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class ConjureItemSpellPlugin : SpellPlugin
    {
        public ConjureItemSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            ushort openTibiaId = Spell.ConjureOpenTibiaId.Value;

            int count = Spell.ConjureCount.Value;

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.RedShimmer) ).Then( () =>
            {
                return Context.AddCommand(new PlayerCreateItemsCommand(player, openTibiaId, 1, count) );
            } );
        }
     }
}