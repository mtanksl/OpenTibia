using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatCommand : Command
    {
        public static CombatCommand BeamAttack(Creature attacker, Offset[] beams, MagicEffectType? magicEffectType, Func<Creature, Creature, int> formula)
        {
            return new CombatCommand()
            {
                Attacker = attacker,

                Target = null,

                Center = attacker.Tile.Position,

                Area = null,

                Beams = beams,

                ProjectileType = null,

                MagicEffectType = magicEffectType,

                OpenTibiaId = null,

                Count = null,

                Formula = formula
            };
        }

        public static CombatCommand AreaAttack(Creature attacker, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Func<Creature, Creature, int> formula)
        {
            return new CombatCommand()
            {
                Attacker = attacker,

                Target = null,

                Center = attacker.Tile.Position,

                Area = area,

                Beams = null,

                ProjectileType = projectileType,

                MagicEffectType = magicEffectType,

                OpenTibiaId = null,

                Count = null,

                Formula = formula
            };
        }

        public static CombatCommand AreaAttack(Creature attacker, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Func<Creature, Creature, int> formula)
        {
            return new CombatCommand()
            {
                Attacker = attacker,

                Target = null,

                Center = center,

                Area = area,

                Beams = null,

                ProjectileType = projectileType,

                MagicEffectType = magicEffectType,

                OpenTibiaId = null,

                Count = null,

                Formula = formula
            };
        }

        public static CombatCommand AreaCreate(Creature attacker, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, Func<Creature, Creature, int> formula)
        {
            return new CombatCommand()
            {
                Attacker = attacker,

                Target = null,

                Center = center,

                Area = area,

                Beams = null,

                ProjectileType = projectileType,

                MagicEffectType = magicEffectType,

                OpenTibiaId = openTibiaId,

                Count = count,

                Formula = formula
            };
        }

        public static CombatCommand AreaCreate(Creature attacker, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, SpecialCondition specialCondition, int[] health, int cooldownInMilliseconds)
        {
            return new CombatCommand()
            {
                Attacker = attacker,

                Target = null,

                Center = center,

                Area = area,

                Beams = null,

                ProjectileType = projectileType,

                MagicEffectType = magicEffectType,

                OpenTibiaId = openTibiaId,

                Count = count,

                SpecialCondition = specialCondition,

                Health = health,

                CooldownInMilliseconds = cooldownInMilliseconds
            };
        }

        public static CombatCommand TargetAttack(Creature attacker, Creature target, ProjectileType? projectileType, MagicEffectType? magicEffectType, Func<Creature, Creature, int> formula)
        {
            return new CombatCommand()
            {
                Attacker = attacker,

                Target = target,

                Center = target.Tile.Position,

                Area = null,

                Beams = null,

                ProjectileType = projectileType,

                MagicEffectType = magicEffectType,

                OpenTibiaId = null,

                Count = null,

                Formula = formula
            };
        }

        public Creature Attacker { get; set; }

        public Creature Target { get; set; }

        public Position Center { get; set; }

        public Offset[] Area { get; set; }

        public Offset[] Beams { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public ushort? OpenTibiaId { get; set; }

        public byte? Count { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public SpecialCondition? SpecialCondition { get; set; }

        public int[] Health { get; set; }

        public int CooldownInMilliseconds { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (ProjectileType != null)
                {
                    Context.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Center, ProjectileType.Value) );
                }

                if (Target != null)
                {
                    if (MagicEffectType != null)
                    {
                        Context.AddCommand(new ShowMagicEffectCommand(Center, MagicEffectType.Value) );
                    }

                    if (SpecialCondition != null)
                    {
                        Context.AddCommand(new CombatConditionCommand(Attacker, Target, SpecialCondition.Value, MagicEffectType, Health, CooldownInMilliseconds) );
                    }
                    else
                    {
                        int health = Formula(Attacker, Target);

                        if (Target != Attacker || health > 0)
                        {
                            Context.AddCommand(new CombatChangeHealthCommand(Attacker, Target, MagicEffectType.ToAnimatedTextColor(), health) );
                        }
                    }
                }

                if (Area != null)
                {
                    foreach (var offset in Area)
                    {
                        Position position = Center.Offset(offset);

                        if (MagicEffectType != null)
                        {
                            Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.Value) );
                        }

                        Tile tile = Context.Server.Map.GetTile(position);

                        if (tile != null)
                        {
                            foreach (var target in tile.GetMonsters().Concat<Creature>(tile.GetPlayers() ).ToList() )
                            {
                                if (SpecialCondition != null)
                                {
                                    Context.AddCommand(new CombatConditionCommand(Attacker, target, SpecialCondition.Value, MagicEffectType, Health, CooldownInMilliseconds) );
                                }
                                else
                                {
                                    int health = Formula(Attacker, target);

                                    if (target != Attacker || health > 0)
                                    {
                                        Context.AddCommand(new CombatChangeHealthCommand(Attacker, target, MagicEffectType.ToAnimatedTextColor(), health) );
                                    }
                                }
                            }

                            if (OpenTibiaId != null && Count != null)
                            {
                                if (tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                                {

                                }
                                else
                                {
                                    Context.AddCommand(new TileCreateItemCommand(tile, OpenTibiaId.Value, Count.Value) ).Then( (item) =>
                                    {
                                        return Context.AddCommand(new ItemDecayDestroyCommand(item, 10000) );
                                    } );
                                }
                            }
                        }
                    }
                }

                if (Beams != null)
                {
                    foreach (var beam in Beams)
                    {
                        Offset offset;

                        if (Attacker.Direction == Direction.North)
                        {
                            offset = new Offset(-beam.X, -beam.Y);
                        }
                        else if (Attacker.Direction == Direction.East)
                        {
                            offset = new Offset(beam.Y, -beam.X);
                        }
                        else if (Attacker.Direction == Direction.West)
                        {
                            offset = new Offset(-beam.Y, beam.X);
                        }
                        else
                        {
                            offset = beam;
                        }

                        Position position = Center.Offset(offset);

                        if (MagicEffectType != null)
                        {
                            Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.Value) );
                        }

                        Tile tile = Context.Server.Map.GetTile(position);

                        if (tile != null)
                        {
                            foreach (var target in tile.GetMonsters().Concat<Creature>(tile.GetPlayers() ).ToList() )
                            {
                                if (SpecialCondition != null)
                                {
                                    Context.AddCommand(new CombatConditionCommand(Attacker, target, SpecialCondition.Value, MagicEffectType, Health, CooldownInMilliseconds) );
                                }
                                else
                                {
                                    int health = Formula(Attacker, target);

                                    if (target != Attacker || health > 0)
                                    {
                                        Context.AddCommand(new CombatChangeHealthCommand(Attacker, target, MagicEffectType.ToAnimatedTextColor(), health) );
                                    }
                                }
                            }

                            if (OpenTibiaId != null && Count != null)
                            {
                                if (tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                                {

                                }
                                else
                                {
                                    Context.AddCommand(new TileCreateItemCommand(tile, OpenTibiaId.Value, Count.Value) ).Then( (item) =>
                                    {
                                        return Context.AddCommand(new ItemDecayDestroyCommand(item, 10000) );
                                    } );
                                }
                            }
                        }
                    }
                }

                resolve();
            } );
        }
    }
}