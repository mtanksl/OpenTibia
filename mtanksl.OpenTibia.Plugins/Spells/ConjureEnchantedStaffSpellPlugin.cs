using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class ConjureEnchantedStaffSpellPlugin : SpellPlugin
    {
        private readonly ushort staff;
        private readonly ushort enchantedStaff;

        public ConjureEnchantedStaffSpellPlugin(Spell spell) : base(spell)
        {
            staff = Context.Server.Values.GetUInt16("values.items.staff");
            enchantedStaff = Context.Server.Values.GetUInt16("values.items.enchantedstaff");
        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Context.AddCommand(new PlayerCountItemsCommand(player, staff, 1) ).Then( (count) =>
            {
                return count > 0 ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
            } );
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.AddCommand(new PlayerCreateItemsCommand(player, enchantedStaff, 1, 1) );

            } ).Then( () =>
            {
                return Context.AddCommand(new PlayerDestroyItemsCommand(player, staff, 1, 1) );
            } );
        }
     }
}