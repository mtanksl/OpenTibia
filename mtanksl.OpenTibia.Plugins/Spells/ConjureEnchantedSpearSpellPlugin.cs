using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class ConjureEnchantedSpearSpellPlugin : SpellPlugin
    {
        private readonly ushort spear;
        private readonly ushort enchantedSpear;

        public ConjureEnchantedSpearSpellPlugin(Spell spell) : base(spell)
        {
            spear = Context.Server.Values.GetUInt16("values.items.spear");
            enchantedSpear = Context.Server.Values.GetUInt16("values.items.enchantedspear");
        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Context.AddCommand(new PlayerCountItemsCommand(player, spear, 1) ).Then( (count) =>
            {
                return count > 0 ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
            } );
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.AddCommand(new PlayerCreateItemsCommand(player, enchantedSpear, 1, 1) );

            } ).Then( () =>
            {
                return Context.AddCommand(new PlayerDestroyItemsCommand(player, spear, 1, 1) );
            } );
        }
     }
}