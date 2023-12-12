using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class GroundshakerSpellPlugin : SpellPlugin
    {
        public GroundshakerSpellPlugin(Spell spell) : base(spell)
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
                                                        new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3),
                                    new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
                new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1),
                new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),
                new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),
                                    new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),
                                                        new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3)
            };

            Item itemWeapon = GetWeapon(player);

            if (itemWeapon == null)
            {
                var formula = GroundshakerFormula(player.Level, player.Skills.Fist, 7);

                return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.GroundShaker,
                        
                    new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
            }
            else
            {
                if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
                {
                    var formula = GroundshakerFormula(player.Level, player.Skills.Sword, itemWeapon.Metadata.Attack.Value);

                    return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.GroundShaker,
                        
                        new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                {
                    var formula = GroundshakerFormula(player.Level, player.Skills.Axe, itemWeapon.Metadata.Attack.Value);

                    return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.GroundShaker,
                        
                        new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
                {
                    var formula = GroundshakerFormula(player.Level, player.Skills.Club, itemWeapon.Metadata.Attack.Value);

                    return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.GroundShaker,
                        
                        new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
             
        public override void Stop()
        {
            
        }
    }
}