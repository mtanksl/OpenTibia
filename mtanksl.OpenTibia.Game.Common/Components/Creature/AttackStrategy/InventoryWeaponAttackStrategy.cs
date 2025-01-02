using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class InventoryWeaponAttackStrategy : IAttackStrategy
    {
        public static readonly InventoryWeaponAttackStrategy Instance = new InventoryWeaponAttackStrategy();

        private InventoryWeaponAttackStrategy()
        {
           
        }

        public bool CanAttack(Creature attacker, Creature target)
        {
            Player player = (Player)attacker;

            Item itemWeapon = Formula.GetWeapon(player);

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
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Wand)
                {
                    if ( !Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
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
                        Item itemAmmunition = Formula.GetAmmunition(player);

                        if (itemAmmunition == null || itemWeapon.Metadata.AmmoType != itemAmmunition.Metadata.AmmoType || !Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                        {
                            return false;
                        }
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

            Item itemWeapon = Formula.GetWeapon(player);

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
                        var formula = Formula.MeleeFormula(player.Level, player.Skills.Sword, itemWeapon.Metadata.Attack.Value, player.Client.FightMode); 

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
                        var formula = Formula.MeleeFormula(player.Level, player.Skills.Club, itemWeapon.Metadata.Attack.Value, player.Client.FightMode);

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
                        var formula = Formula.MeleeFormula(player.Level, player.Skills.Axe, itemWeapon.Metadata.Attack.Value, player.Client.FightMode); 

                        return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                            
                            new MeleeAttack(formula.Min, formula.Max) ) );
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
                            var formula = Formula.WandFormula(itemWeapon.Metadata.AttackStrength.Value, itemWeapon.Metadata.AttackVariation.Value);

                            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target,
                            
                                new DistanceAttack(itemWeapon.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
                        } );
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Distance)
                {
                    if (itemWeapon.Metadata.AmmoType == null)
                    {
                        WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                        Promise promise;

                        if ( !Context.Current.Server.Config.GameplayRemoveWeaponCharges)
                        {
                            promise = Promise.Completed;
                        }
                        else
                        {
                            promise = Context.Current.AddCommand(new ItemDecrementCommand(itemWeapon, 1) );
                        }

                        if (plugin != null)
                        {
                            return promise.Then( () =>
                            {
                                return plugin.OnUseWeapon(player, target, itemWeapon);
                            } );
                        }
                        else
                        {
                            return promise.Then( () =>
                            {
                                var formula = Formula.DistanceFormula(player.Level, player.Skills.Distance, itemWeapon.Metadata.Attack.Value, player.Client.FightMode);

                                return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                                
                                    new DistanceAttack(itemWeapon.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
                            } );
                        }
                    }
                    else
                    {
                        Item itemAmmunition = Formula.GetAmmunition(player);

                        AmmunitionPlugin plugin = Context.Current.Server.Plugins.GetAmmunitionPlugin(itemAmmunition.Metadata.OpenTibiaId);

                        Promise promise;

                        if ( !Context.Current.Server.Config.GameplayRemoveWeaponAmmunition)
                        {
                            promise = Promise.Completed;
                        }
                        else
                        {
                            promise = Context.Current.AddCommand(new ItemDecrementCommand(itemAmmunition, 1) );
                        }

                        if (plugin != null)
                        {
                            return promise.Then( () =>
                            {
                                return plugin.OnUseAmmunition(player, target, itemWeapon, itemAmmunition);
                            } );                        
                        }
                        else
                        {
                            return promise.Then( () =>
                            {
                                var formula = Formula.DistanceFormula(player.Level, player.Skills.Distance, itemAmmunition.Metadata.Attack.Value, player.Client.FightMode);

                                return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                                
                                    new DistanceAttack(itemAmmunition.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
                            } );
                        }                        
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                var formula = Formula.MeleeFormula(player.Level, player.Skills.Fist, 7, player.Client.FightMode); 

                return Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                    
                    new MeleeAttack(formula.Min, formula.Max) ) );
            }
        }
    }
}