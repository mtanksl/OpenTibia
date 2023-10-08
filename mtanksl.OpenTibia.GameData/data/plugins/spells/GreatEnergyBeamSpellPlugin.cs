using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;

namespace mtanksl.OpenTibia.GameData.Plugins.Spells
{
    public class GreatEnergyBeamSpellPlugin : SpellPlugin
    {
        public GreatEnergyBeamSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override void Start()
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResult(true);
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            Offset[] area = new Offset[]
            {
                new Offset(0, 1),
                new Offset(0, 2),
                new Offset(0, 3),
                new Offset(0, 4),
                new Offset(0, 5),
                new Offset(0, 6),
                new Offset(0, 7)
            };

            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 4, 0, 7, 0);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, true, player.Tile.Position, area, null, MagicEffectType.EnergyArea, 
                        
                new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
        }
             
        public override void Stop()
        {
            
        }
    }
}