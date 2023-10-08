using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace mtanksl.OpenTibia.GameData.Plugins.Ammunitions
{
    public class PoisonArrowAmmunitionPlugin : AmmunitionPlugin
    {
        public PoisonArrowAmmunitionPlugin(Ammunition ammunition) : base(ammunition)
        {

        }

        public override void Start()
        {
            
        }

        public override Promise OnUseAmmunition(Player player, Creature target, Item weapon, Item ammunition)
        {
           var formula = DistanceFormula(player.Level, player.Skills.Distance, ammunition.Metadata.Attack.Value, player.Client.FightMode);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target, 

                new DistanceAttack(ammunition.Metadata.ProjectileType.Value, formula.Min, formula.Max),
                                                                                                                                 
                new DamageCondition(SpecialCondition.Poisoned, MagicEffectType.GreenRings, AnimatedTextColor.Green, new[] { 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) );
        }

        public override void Stop()
        {
           
        }        
    }
}