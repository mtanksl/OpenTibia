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

        public async PromiseResult<bool> CanAttack(Creature attacker, Creature target)
        {
            Player player = (Player)attacker;

            Item itemWeapon = Formula.GetWeapon(player);

            if (itemWeapon != null)
            {
                if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
                {
                    if ( !player.Tile.Position.IsNextTo(target.Tile.Position) )
                    {
                        return false;
                    }

                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        if (player.Level < plugin.Weapon.Level || (plugin.Weapon.Vocations != null && !plugin.Weapon.Vocations.Contains(player.Vocation) ) )
                        {
                            return false;
                        }

                        if ( !await plugin.OnUsingWeapon(player, target, itemWeapon) )
                        {
                            return false;
                        }
                    }                    
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
                {
                    if ( !player.Tile.Position.IsNextTo(target.Tile.Position) )
                    {
                        return false;
                    }

                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        if (player.Level < plugin.Weapon.Level || (plugin.Weapon.Vocations != null && !plugin.Weapon.Vocations.Contains(player.Vocation) ) )
                        {
                            return false;
                        }

                        if ( !await plugin.OnUsingWeapon(player, target, itemWeapon) )
                        {
                            return false;
                        }
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                {
                    if ( !player.Tile.Position.IsNextTo(target.Tile.Position) )
                    {
                        return false;
                    }

                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        if (player.Level < plugin.Weapon.Level || (plugin.Weapon.Vocations != null && !plugin.Weapon.Vocations.Contains(player.Vocation) ) )
                        {
                            return false;
                        }

                        if ( !await plugin.OnUsingWeapon(player, target, itemWeapon) )
                        {
                            return false;
                        }
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Wand)
                {
                    if (itemWeapon.Metadata.Range != null && !player.Tile.Position.IsInRange(target.Tile.Position, itemWeapon.Metadata.Range.Value) )
                    {
                        return false;
                    }

                    if ( !Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                    {
                        return false;
                    }

                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        if (player.Level < plugin.Weapon.Level || player.Mana < plugin.Weapon.Mana || (plugin.Weapon.Vocations != null && !plugin.Weapon.Vocations.Contains(player.Vocation) ) )
                        {
                            return false;
                        }

                        if ( !await plugin.OnUsingWeapon(player, target, itemWeapon) )
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Distance)
                {     
                    if (itemWeapon.Metadata.Range != null && !player.Tile.Position.IsInRange(target.Tile.Position, itemWeapon.Metadata.Range.Value) )
                    {
                        return false;
                    }

                    if ( !Context.Current.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
                    {
                        return false;
                    }
                                                            
                    if (itemWeapon.Metadata.AmmoType != null)
                    {
                        Item itemAmmunition = Formula.GetAmmunition(player);

                        if (itemAmmunition != null && itemAmmunition.Metadata.AmmoType == itemWeapon.Metadata.AmmoType)
                        {
                            WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                            if (plugin != null)
                            {
                                if (player.Level < plugin.Weapon.Level || (plugin.Weapon.Vocations != null && !plugin.Weapon.Vocations.Contains(player.Vocation) ) )
                                {
                                    return false;
                                }
                            }

                            AmmunitionPlugin ammunitionPlugin = Context.Current.Server.Plugins.GetAmmunitionPlugin(itemAmmunition.Metadata.OpenTibiaId);

                            if (ammunitionPlugin != null)
                            {
                                if (player.Level < ammunitionPlugin.Ammunition.Level)
                                {
                                    return false;
                                }

                                if ( !await ammunitionPlugin.OnUsingAmmunition(player, target, itemWeapon, itemAmmunition) )
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }                        
                    }
                    else
                    {
                        WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                        if (plugin != null)
                        {
                            if (player.Level < plugin.Weapon.Level || (plugin.Weapon.Vocations != null && !plugin.Weapon.Vocations.Contains(player.Vocation) ) )
                            {
                                return false;
                            }

                            if ( !await plugin.OnUsingWeapon(player, target, itemWeapon) )
                            {
                                return false;
                            }
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

        public async Promise Attack(Creature attacker, Creature target)
        {
            Player player = (Player)attacker;

            Item itemWeapon = Formula.GetWeapon(player);

            if (itemWeapon != null)
            {
                if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
                {
                    await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.Sword, 1) );
                    
                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        await plugin.OnUseWeapon(player, target, itemWeapon);
                    }
                    else
                    {
                        var formula = Formula.MeleeFormula(player.Level, player.Skills.GetSkillLevel(Skill.Sword), itemWeapon.Metadata.Attack.Value, player.Client.FightMode); 

                        await Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target,
                            
                            new MeleeAttack(formula.Min, formula.Max) ) );
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
                {
                    await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.Club, 1) );
                    
                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        await plugin.OnUseWeapon(player, target, itemWeapon);
                    }
                    else
                    {
                        var formula = Formula.MeleeFormula(player.Level, player.Skills.GetSkillLevel(Skill.Club), itemWeapon.Metadata.Attack.Value, player.Client.FightMode);

                        await Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                            
                            new MeleeAttack(formula.Min, formula.Max) ) );
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
                {
                    await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.Axe, 1) );
                    
                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        await plugin.OnUseWeapon(player, target, itemWeapon);
                    }
                    else
                    {
                        var formula = Formula.MeleeFormula(player.Level, player.Skills.GetSkillLevel(Skill.Axe), itemWeapon.Metadata.Attack.Value, player.Client.FightMode); 

                        await Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                            
                            new MeleeAttack(formula.Min, formula.Max) ) );
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Wand)
                {
                    WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.MagicLevel, (ulong)plugin.Weapon.Mana) );

                        await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana - plugin.Weapon.Mana) );

                        await plugin.OnUseWeapon(player, target, itemWeapon);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (itemWeapon.Metadata.WeaponType == WeaponType.Distance)
                {
                    if (itemWeapon.Metadata.AmmoType != null)
                    {
                        Item itemAmmunition = Formula.GetAmmunition(player);

                        if (itemAmmunition != null)
                        {
                            await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.Distance, 1) );

                            if (Context.Current.Server.Config.GameplayRemoveWeaponAmmunition)
                            {
                                if (itemAmmunition.Metadata.BreakChance != null && Context.Current.Server.Randomization.HasProbability(1.0 / itemAmmunition.Metadata.BreakChance.Value) )
                                {
                                    await Context.Current.AddCommand(new ItemDecrementCommand(itemAmmunition, 1) );
                                }
                                else
                                {
                                    if (itemAmmunition.Metadata.AmmoAction == AmmoAction.Remove)
                                    {
                                        await Context.Current.AddCommand(new ItemDecrementCommand(itemAmmunition, 1) );
                                    }
                                    else if (itemAmmunition.Metadata.AmmoAction == AmmoAction.Move)
                                    {
                                        await Context.Current.AddCommand(new ItemDecrementCommand(itemAmmunition, 1) );

                                        await Context.Current.AddCommand(new TileCreateItemOrIncrementCommand(target.Tile, itemAmmunition.Metadata.OpenTibiaId, 1) );
                                    }
                                }
                            }

                            /*
                            WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                            if (plugin != null)
                            {
                                await plugin.OnUseWeapon(player, target, itemWeapon);
                            }
                            else 
                            {

                            }
                            */

                            AmmunitionPlugin ammunitionPlugin = Context.Current.Server.Plugins.GetAmmunitionPlugin(itemAmmunition.Metadata.OpenTibiaId);

                            if (ammunitionPlugin != null)
                            {
                                await ammunitionPlugin.OnUseAmmunition(player, target, itemWeapon, itemAmmunition);
                            }
                            else
                            {                                 
                                var formula = Formula.DistanceFormula(player.Level, player.Skills.GetSkillLevel(Skill.Distance), itemAmmunition.Metadata.Attack.Value, player.Client.FightMode);

                                await Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                                
                                    new DistanceAttack(itemAmmunition.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }                        
                    }
                    else
                    {
                        await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.Distance, 1) );

                        if (Context.Current.Server.Config.GameplayRemoveWeaponCharges)
                        {
                            if (itemWeapon.Metadata.BreakChance != null && Context.Current.Server.Randomization.HasProbability(1.0 / itemWeapon.Metadata.BreakChance.Value) )
                            {
                                await Context.Current.AddCommand(new ItemDecrementCommand(itemWeapon, 1) );
                            }
                            else
                            {
                                if (itemWeapon.Metadata.AmmoAction == AmmoAction.Remove)
                                {
                                    await Context.Current.AddCommand(new ItemDecrementCommand(itemWeapon, 1) );
                                }
                                else if (itemWeapon.Metadata.AmmoAction == AmmoAction.Move)
                                {
                                    await Context.Current.AddCommand(new ItemDecrementCommand(itemWeapon, 1) );

                                    await Context.Current.AddCommand(new TileCreateItemOrIncrementCommand(target.Tile, itemWeapon.Metadata.OpenTibiaId, 1) );
                                }
                            }
                        }

                        WeaponPlugin plugin = Context.Current.Server.Plugins.GetWeaponPlugin(itemWeapon.Metadata.OpenTibiaId);

                        if (plugin != null)
                        {
                            await plugin.OnUseWeapon(player, target, itemWeapon);
                        }
                        else
                        {
                            var formula = Formula.DistanceFormula(player.Level, player.Skills.GetSkillLevel(Skill.Distance), itemWeapon.Metadata.Attack.Value, player.Client.FightMode);

                            await Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                                
                                new DistanceAttack(itemWeapon.Metadata.ProjectileType.Value, formula.Min, formula.Max) ) );
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
                await Context.Current.AddCommand(new PlayerAddSkillPointsCommand(player, Skill.Fist, 1) );

                var formula = Formula.MeleeFormula(player.Level, player.Skills.GetSkillLevel(Skill.Fist), 7, player.Client.FightMode); 

                await Context.Current.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                    
                    new MeleeAttack(formula.Min, formula.Max) ) );
            }
        }
    }
}