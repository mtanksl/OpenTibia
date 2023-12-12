using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class IntenseHealingSpellPlugin : SpellPlugin
    {
        public IntenseHealingSpellPlugin(Spell spell) : base(spell)
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
            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 3.184, 20, 5.59, 35);
                    
            return Context.AddCommand(new CreatureAttackCreatureCommand(player, player, 
                        
                new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
        }
             
        public override void Stop()
        {
            
        }
    }
}