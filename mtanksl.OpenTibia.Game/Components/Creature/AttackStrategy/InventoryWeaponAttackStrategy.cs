using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class InventoryWeaponAttackStrategy : IAttackStrategy
    {
        private class Weapon
        {
            public int Level { get; set; }

            public int Mana { get; set; }

            public int AttackStrength { get; set; }

            public int AttackVariation { get; set; }

            public Vocation[] Vocations { get; set; }
        }

        private static Dictionary<ushort, Weapon> weapons = new Dictionary<ushort, Weapon>()
        {
            [2190 /* Wand of vortex */] = new Weapon()
            {
                Level = 6,

                Mana = 2,

                AttackStrength = 13,

                AttackVariation = 5,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer }
            },

            [2182 /* snakebite rod */] = new Weapon()
            {
                Level = 6,

                Mana = 2,

                AttackStrength = 13,

                AttackVariation = 5,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid }
            },

            //TODO: More items
        };

        private TimeSpan cooldown;

        public InventoryWeaponAttackStrategy(TimeSpan cooldown)
        {
            this.cooldown = cooldown;
        }

        public TimeSpan Cooldown
        {
            get
            {
                return cooldown;
            }
        }

        public bool CanAttack(Creature attacker, Creature target)
        {
            Player player = (Player)attacker;

            Item item = GetWeapon(player);

            if (item != null)
            {
                Weapon weapon;

                if (weapons.TryGetValue(item.Metadata.OpenTibiaId, out weapon) )
                {
                    if (player.Level < weapon.Level || player.Mana < weapon.Mana || (weapon.Vocations != null && !weapon.Vocations.Contains(player.Vocation) ) )
                    {
                        return false;
                    }
                }

                if (item.Metadata.Range != null && !player.Tile.Position.IsInRange(target.Tile.Position, item.Metadata.Range.Value) )
                {
                    return false;
                }

                if (item.Metadata.WeaponType == WeaponType.Distance)
                {
                    if (item.Metadata.AmmoType == null)
                    {
                        if (Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                        {
                            return true;
                        }

                        return false;
                    }
                    else
                    {
                        Item item2 = GetAmmunition(player);

                        if (item2 != null)
                        {
                            if (item.Metadata.AmmoType == item2.Metadata.AmmoType)
                            {
                                if (Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                                {
                                    return true;
                                }
                            }
                        }

                        return false;
                    }                  
                }
                else if (item.Metadata.WeaponType == WeaponType.Wand)
                {
                    if (Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                    {
                        return true;
                    }

                    return false;
                }
            }

            if (player.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return true;
            }

            return false;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            Player player = (Player)attacker;

            Item item = GetWeapon(player);

            Attack attack;

            if (item != null)
            {
                Weapon weapon;

                weapons.TryGetValue(item.Metadata.OpenTibiaId, out weapon);

                if (item.Metadata.WeaponType == WeaponType.Sword)
                {
                    var formula = MeleeFormula(player.Level, player.Skills.Sword, item.Metadata.Attack.Value, player.Client.FightMode); 
                    
                    attack = new MeleeAttack(formula.Min, formula.Max);
                }
                else if (item.Metadata.WeaponType == WeaponType.Club)
                {
                    var formula = MeleeFormula(player.Level, player.Skills.Club, item.Metadata.Attack.Value, player.Client.FightMode);
                    
                    attack = new MeleeAttack(formula.Min, formula.Max);
                }
                else if (item.Metadata.WeaponType == WeaponType.Axe)
                {
                    var formula = MeleeFormula(player.Level, player.Skills.Axe, item.Metadata.Attack.Value, player.Client.FightMode); 
                    
                    attack = new MeleeAttack(formula.Min, formula.Max);
                }
                else if (item.Metadata.WeaponType == WeaponType.Distance)
                {
                    if (item.Metadata.AmmoType == null)
                    {
                        var formula = DistanceFormula(player.Level, player.Skills.Distance, item.Metadata.Attack.Value, player.Client.FightMode); 
                        
                        attack = new DistanceAttack(item.Metadata.ProjectileType.Value, formula.Min, formula.Max);
                    }
                    else
                    {
                        Item item2 = GetAmmunition(player);

                        var formula = DistanceFormula(player.Level, player.Skills.Distance, item2.Metadata.Attack.Value, player.Client.FightMode); 
                        
                        attack = new DistanceAttack(item2.Metadata.ProjectileType.Value, formula.Min, formula.Max);
                    }
                }
                else if (item.Metadata.WeaponType == WeaponType.Wand)
                {
                    var min = weapon.AttackStrength - weapon.AttackVariation;

                    var max = weapon.AttackStrength + weapon.AttackVariation;

                    attack = new DistanceAttack(item.Metadata.ProjectileType.Value, min, max);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                var formula = MeleeFormula(player.Level, player.Skills.Fist, 7, player.Client.FightMode); 
                
                attack = new MeleeAttack(formula.Min, formula.Max);
            }

            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, attack) );
        }

        private static Item GetWeapon(Player player)
        {
            Item item = player.Inventory.GetContent( (byte)Slot.Left) as Item;

            if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe || item.Metadata.WeaponType == WeaponType.Distance || item.Metadata.WeaponType == WeaponType.Wand) )
            {
                return item;
            }

            item = player.Inventory.GetContent( (byte)Slot.Right) as Item;

            if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe || item.Metadata.WeaponType == WeaponType.Distance || item.Metadata.WeaponType == WeaponType.Wand) )
            {
                return item;
            }

            return null;
        }

        private static Item GetAmmunition(Player player)
        {
            Item item = player.Inventory.GetContent( (byte)Slot.Extra) as Item;

            if (item != null && item.Metadata.WeaponType == WeaponType.Ammo)
            {
                return item;
            }

            return null;
        }

        private static (int Min, int Max) MeleeFormula(int level, int skill, int attack, FightMode fightMode)
        {
            int min = 0;

            int max = (int)Math.Floor(0.085 * (fightMode == FightMode.Offensive ? 1 : fightMode == FightMode.Balanced ? 0.75 : 0.5) * skill * attack) + (int)Math.Floor(level / 5.0);

            return (min, max);
        }

        private static (int Min, int Max) DistanceFormula(int level, int skill, int attack, FightMode fightMode)
        {
            int min = (int)Math.Floor(level / 5.0);

            int max = (int)Math.Floor(0.09 * (fightMode == FightMode.Offensive ? 1 : fightMode == FightMode.Balanced ? 0.75 : 0.5) * skill * attack) + min;

            return (min, max);
        }
    }
}