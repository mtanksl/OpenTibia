using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class DeathStrikeSpellPlugin : SpellPlugin
    {
        public DeathStrikeSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            Offset[] area = new Offset[]
            {
                new Offset(0, 1)
            };

            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 45, 10);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, true, player.Tile.Position, area, null, MagicEffectType.MortArea, 
                        
                new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
        }
    }
}