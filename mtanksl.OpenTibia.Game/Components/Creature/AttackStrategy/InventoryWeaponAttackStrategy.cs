using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class InventoryWeaponAttackStrategy : IAttackStrategy
    {
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
                WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                if (plugin != null)
                {
                    if (player.Level < plugin.Weapon.Level || player.Mana < plugin.Weapon.Mana || (plugin.Weapon.Vocations != null && !plugin.Weapon.Vocations.Contains(player.Vocation) ) )
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
                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        return plugin.OnUseWeapon(player, target, itemWeapon);
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
                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        return plugin.OnUseWeapon(player, target, itemWeapon);
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
                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        return plugin.OnUseWeapon(player, target, itemWeapon);
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
                        WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                        if (plugin != null)
                        {
                            return Context.Current.AddCommand(new ItemDecrementCommand(itemWeapon, 1) ).Then( () =>
                            {
                                return plugin.OnUseWeapon(player, target, itemWeapon);
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

                        AmmunitionPlugin plugin = Context.Current.Server.Plugins.GetAmmunitionPlugin(itemAmmunition.Metadata.OpenTibiaId);

                        if (plugin != null)
                        {
                            return Context.Current.AddCommand(new ItemDecrementCommand(itemAmmunition, 1) ).Then( () =>
                            {
                                return plugin.OnUseAmmunition(player, target, itemWeapon, itemAmmunition);
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
                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        return Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - plugin.Weapon.Mana) ).Then( () =>
                        {
                            return plugin.OnUseWeapon(player, target, itemWeapon);
                        } );
                    }
                    else
                    {
                        return Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - plugin.Weapon.Mana) ).Then( () =>
                        {
                            var formula = WandFormula(itemWeapon.Metadata.AttackStrength.Value, itemWeapon.Metadata.AttackVariation.Value);

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
    }
}