using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class EnergyWaveSpellPlugin : SpellPlugin
    {
        public EnergyWaveSpellPlugin(Spell spell) : base(spell)
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
                                    new Offset(0, 1),
                                    new Offset(0, 2),
                new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
                new Offset(-1, 5), new Offset(0, 5), new Offset(1, 5),
            };

            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 4.5, 0, 9, 0);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, true, player.Tile.Position, area, null, MagicEffectType.EnergyArea, 
                        
                new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
        }
    }
}