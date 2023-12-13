﻿using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class FierceBerserkSpellPlugin : SpellPlugin
    {
        public FierceBerserkSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override void Start()
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
                new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1)
            };

            Item itemWeapon = GetWeapon(player);

            if (itemWeapon == null)
            {
                var formula = FierceBerserkFormula(player.Level, player.Skills.Fist, 7);

                return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                    new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
            }
            else
            {
                if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
                {
                    var formula = FierceBerserkFormula(player.Level, player.Skills.Sword, itemWeapon.Metadata.Attack.Value);

                    return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                        new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                {
                    var formula = FierceBerserkFormula(player.Level, player.Skills.Axe, itemWeapon.Metadata.Attack.Value);

                    return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
                        new SimpleAttack(null, null, AnimatedTextColor.DarkRed, formula.Min, formula.Max) ) );
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
                {
                    var formula = FierceBerserkFormula(player.Level, player.Skills.Club, itemWeapon.Metadata.Attack.Value);

                    return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.BlackSpark,
                        
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