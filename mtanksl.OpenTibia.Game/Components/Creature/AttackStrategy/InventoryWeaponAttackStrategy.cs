using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class InventoryWeaponAttackStrategy : IAttackStrategy
    {
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

            Item weapon = GetWeapon(player);

            if (weapon != null)
            {
                if (weapon.Metadata.WeaponType == WeaponType.Distance)
                {
                    //TODO: Has vocation?
                    //TODO: Has ammo?

                    if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
                    {
                        return true;
                    }
                }
                else if (weapon.Metadata.WeaponType == WeaponType.Wand)
                {
                    //TODO: Has mana?

                    if (Context.Current.Server.Pathfinding.CanThrow(attacker.Tile.Position, target.Tile.Position) )
                    {
                        return true;
                    }
                }
            }

            //TODO: Has vocation?

            if (attacker.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return true;
            }

            return false;
        }

        public Promise Attack(Creature attacker, Creature target)
        {
            Player player = (Player)attacker;

            Item weapon = GetWeapon(player);

            Attack attack;

            if (weapon != null)
            {
                if (weapon.Metadata.WeaponType == WeaponType.Sword)
                {
                    var formula = MeleeFormula(player.Level, player.Skills.Sword, weapon.Metadata.Attack ?? 0, player.Client.FightMode);

                    attack = new MeleeAttack(formula.Min, formula.Max);
                }
                else if (weapon.Metadata.WeaponType == WeaponType.Club)
                {
                    var formula = MeleeFormula(player.Level, player.Skills.Club, weapon.Metadata.Attack ?? 0, player.Client.FightMode);

                    attack = new MeleeAttack(formula.Min, formula.Max);
                }
                else if (weapon.Metadata.WeaponType == WeaponType.Axe)
                {
                    var formula = MeleeFormula(player.Level, player.Skills.Axe, weapon.Metadata.Attack ?? 0, player.Client.FightMode);

                    attack = new MeleeAttack(formula.Min, formula.Max);
                }
                else if (weapon.Metadata.WeaponType == WeaponType.Distance)
                {
                    var formula = DistanceFormula(player.Level, player.Skills.Distance, weapon.Metadata.Attack ?? 0, player.Client.FightMode);

                    attack = new DistanceAttack(ProjectileType.Arrow, formula.Min, formula.Max);
                }
                else if (weapon.Metadata.WeaponType == WeaponType.Wand)
                {
                    var formula = DistanceFormula(player.Level, player.Skills.Distance, weapon.Metadata.Attack ?? 0, player.Client.FightMode);

                    attack = new DistanceAttack(ProjectileType.Energy, formula.Min, formula.Max);
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

            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target, attack) );
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