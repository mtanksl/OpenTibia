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

            public Func<Player, Creature, Item, Promise> Callback { get; set; }
        }

        private class Ammunition
        {
            public Func<Player, Creature, Item, Item, Promise> Callback { get; set; }
        }

        private static Dictionary<ushort, Weapon> weapons = new Dictionary<ushort, Weapon>()
        {
            [2187 /* Wand of inferno */] = new Weapon()
            {
                Level = 33,

                Mana = 13,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

                AttackStrength = 65,

                AttackVariation = 9
            },

            [2188 /* Wand of plague */] = new Weapon()
            {
                Level = 19,

                Mana = 5,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

                AttackStrength = 30,

                AttackVariation = 7
            },

            [2189 /* Wand of cosmic energy */] = new Weapon()
            {
                Level = 26,

                Mana = 8,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

                AttackStrength = 45,

                AttackVariation = 8
            },

            [2190 /* Wand of vortex */] = new Weapon()
            {
                Level = 7,

                Mana = 2,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

                AttackStrength = 13,

                AttackVariation = 5
            },

            [2191 /* Wand of dragonbreath */] = new Weapon()
            {
                Level = 13,

                Mana = 3,

                Vocations = new[] { Vocation.Sorcerer, Vocation.MasterSorcerer },

                AttackStrength = 19,

                AttackVariation = 6
            },
            
            [2181 /* Quagmire rod */] = new Weapon()
            {
                Level = 26,

                Mana = 8,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

                AttackStrength = 45,

                AttackVariation = 8
            },

            [2182 /* Snakebite rod */] = new Weapon()
            {
                Level = 6,

                Mana = 2,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

                AttackStrength = 13,

                AttackVariation = 5
            },

            [2183 /* Tempest rod */] = new Weapon()
            {
                Level = 33,

                Mana = 13,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

                AttackStrength = 65,

                AttackVariation = 9
            },

            [2185 /* Volcanic rod */] = new Weapon()
            {
                Level = 19,

                Mana = 5,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

                AttackStrength = 30,

                AttackVariation = 7
            },

            [2186 /* Moonlight rod */] = new Weapon()
            {
                Level = 13,

                Mana = 3,

                Vocations = new[] { Vocation.Druid, Vocation.ElderDruid },

                AttackStrength = 19,

                AttackVariation = 6
            },

            [7366 /* Viper star */] = new Weapon()
            {
                Callback = (attacker, target, weapon) =>
                {
                    var formula = DistanceFormula(attacker.Level, attacker.Skills.Distance, weapon.Metadata.Attack.Value, attacker.Client.FightMode);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target,

                        new DistanceAttack(weapon.Metadata.ProjectileType.Value, formula.Min, formula.Max),

                        new DamageCondition(SpecialCondition.Poisoned, MagicEffectType.GreenRings, AnimatedTextColor.Green, new[] { 2, 2, 2, 2, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) );
                }
            },
        };

        private static Dictionary<ushort, Ammunition> ammunitions = new Dictionary<ushort, Ammunition>()
        {
            [2545 /* Poison arrow */] = new Ammunition()
            {
                Callback = (attacker, target, weapon, ammunition) =>
                {
                    var formula = DistanceFormula(attacker.Level, attacker.Skills.Distance, ammunition.Metadata.Attack.Value, attacker.Client.FightMode);

                    return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, 

                        new DistanceAttack(ammunition.Metadata.ProjectileType.Value, formula.Min, formula.Max),
                                                                                                                                 
                        new DamageCondition(SpecialCondition.Poisoned, MagicEffectType.GreenRings, AnimatedTextColor.Green, new[] { 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) );
                }
            },

            [2546 /* Burst arrow */] = new Ammunition()
            {
                Callback = (attacker, target, weapon, ammunition) =>
                {
                    Offset[] area = new Offset[]
                    {
                        new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                        new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                        new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),
                    };

                    var formula = DistanceFormula(attacker.Level, attacker.Skills.Distance, ammunition.Metadata.Attack.Value, attacker.Client.FightMode);

                    return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, target.Tile.Position, area, ammunition.Metadata.ProjectileType.Value, MagicEffectType.FireArea, 
                        
                        new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
                }
            }
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

            Item itemWeapon = GetWeapon(player);

            if (itemWeapon != null)
            {
                Weapon weapon;

                if (weapons.TryGetValue(itemWeapon.Metadata.OpenTibiaId, out weapon) )
                {
                    if (player.Level < weapon.Level || player.Mana < weapon.Mana || (weapon.Vocations != null && !weapon.Vocations.Contains(player.Vocation) ) )
                    {
                        return false;
                    }
                }

                if (itemWeapon.Metadata.Range != null && !player.Tile.Position.IsInRange(target.Tile.Position, itemWeapon.Metadata.Range.Value) )
                {
                    return false;
                }

                if (itemWeapon.Metadata.WeaponType == WeaponType.Sword || itemWeapon.Metadata.WeaponType == WeaponType.Club || itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                {
                    if ( !player.Tile.Position.IsNextTo(target.Tile.Position) )
                    {
                        return false;
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Distance)
                {
                    if (itemWeapon.Metadata.AmmoType == null)
                    {
                        if ( !Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                        {
                            return false;
                        }
                    }
                    else
                    {
                        Item itemAmmunition = GetAmmunition(player);

                        if (itemAmmunition == null || itemWeapon.Metadata.AmmoType != itemAmmunition.Metadata.AmmoType || !Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                        {
                            return false;
                        }
                    }                  
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Wand)
                {
                    if ( !Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                    {
                        return false;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                if ( !player.Tile.Position.IsNextTo(target.Tile.Position) )
                {
                    return false;
                }
            }

            return true;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            Player player = (Player)attacker;

            Item itemWeapon = GetWeapon(player);

            if (itemWeapon != null)
            {
                if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
                {
                    Weapon weapon;

                    if (weapons.TryGetValue(itemWeapon.Metadata.OpenTibiaId, out weapon) && weapon.Callback != null)
                    {
                        return weapon.Callback(player, target, itemWeapon);
                    }
                    else
                    {
                        var formula = MeleeFormula(player.Level, player.Skills.Sword, itemWeapon.Metadata.Attack.Value, player.Client.FightMode); 

                        return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target,
                            
                            new MeleeAttack(formula.Min, formula.Max) ) );
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
                {
                    Weapon weapon;

                    if (weapons.TryGetValue(itemWeapon.Metadata.OpenTibiaId, out weapon) && weapon.Callback != null)
                    {
                        return weapon.Callback(player, target, itemWeapon);
                    }
                    else
                    {
                        var formula = MeleeFormula(player.Level, player.Skills.Club, itemWeapon.Metadata.Attack.Value, player.Client.FightMode);

                        return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                            
                            new MeleeAttack(formula.Min, formula.Max) ) );
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                {
                    Weapon weapon;

                    if (weapons.TryGetValue(itemWeapon.Metadata.OpenTibiaId, out weapon) && weapon.Callback != null)
                    {
                        return weapon.Callback(player, target, itemWeapon);
                    }
                    else
                    {
                        var formula = MeleeFormula(player.Level, player.Skills.Axe, itemWeapon.Metadata.Attack.Value, player.Client.FightMode); 

                        return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                            
                            new MeleeAttack(formula.Min, formula.Max) ) );
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Distance)
                {
                    if (itemWeapon.Metadata.AmmoType == null)
                    {
                        Weapon weapon;

                        if (weapons.TryGetValue(itemWeapon.Metadata.OpenTibiaId, out weapon) && weapon.Callback != null)
                        {
                            return Context.Current.AddCommand(new ItemDecrementCommand(itemWeapon, 1) ).Then( () =>
                            {
                                return weapon.Callback(player, target, itemWeapon);
                            } );
                        }
                        else
                        {
                            return Context.Current.AddCommand(new ItemDecrementCommand(itemWeapon, 1) ).Then( () =>
                            {
                                var formula = DistanceFormula(player.Level, player.Skills.Distance, itemWeapon.Metadata.Attack.Value, player.Client.FightMode);

                                return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                                
                                    new DistanceAttack(itemWeapon.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
                            } );
                        }
                    }
                    else
                    {
                        Item itemAmmunition = GetAmmunition(player);

                        Ammunition ammunition;

                        if (ammunitions.TryGetValue(itemAmmunition.Metadata.OpenTibiaId, out ammunition) && ammunition.Callback != null)
                        {
                            return Context.Current.AddCommand(new ItemDecrementCommand(itemAmmunition, 1) ).Then( () =>
                            {
                                return ammunition.Callback(player, target, itemWeapon, itemAmmunition);
                            } );                        
                        }
                        else
                        {
                            return Context.Current.AddCommand(new ItemDecrementCommand(itemAmmunition, 1) ).Then( () =>
                            {
                                var formula = DistanceFormula(player.Level, player.Skills.Distance, itemAmmunition.Metadata.Attack.Value, player.Client.FightMode);

                                return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                                
                                    new DistanceAttack(itemAmmunition.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
                            } );
                        }                        
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Wand)
                {
                    Weapon weapon;

                    if (weapons.TryGetValue(itemWeapon.Metadata.OpenTibiaId, out weapon) && weapon.Callback != null)
                    {
                        return Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - weapon.Mana) ).Then( () =>
                        {
                            return weapon.Callback(player, target, itemWeapon);
                        } );
                    }
                    else
                    {
                        return Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - weapon.Mana) ).Then( () =>
                        {
                            var formula = WandFormula(weapon.AttackStrength, weapon.AttackVariation);

                            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target,
                            
                                new DistanceAttack(itemWeapon.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
                        } );
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                var formula = MeleeFormula(player.Level, player.Skills.Fist, 7, player.Client.FightMode); 

                return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                    
                    new MeleeAttack(formula.Min, formula.Max) ) );
            }
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

        private static (int Min, int Max) WandFormula(int attackStrength, int attackVariation)
        {
            int min = attackStrength - attackVariation;

            int max = attackStrength + attackVariation;

            return (min, max);
        }
    }
}