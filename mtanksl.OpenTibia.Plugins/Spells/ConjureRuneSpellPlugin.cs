using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class ConjureRuneSpellPlugin : SpellPlugin
    {
        private readonly ushort blankRune;

        public ConjureRuneSpellPlugin(Spell spell) : base(spell)
        {
            blankRune = Context.Server.Values.GetUInt16("values.items.blankrune");
        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Context.AddCommand(new PlayerCountItemsCommand(player, blankRune, 1) ).Then( (count) =>
            {
                return count > 0 ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
            } );
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
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