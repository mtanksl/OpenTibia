using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackAreaBuilder
    {
        public Creature Attacker { get; set; }

        public Position Center { get; set; }

        public Offset[] Area { get; set; }

        public Direction? Direction { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public ushort? OpenTibiaId { get; set; }

        public byte? Count { get; set; }

        public DamageDto Formula { get; set; }

        public ConditionDto Condition { get; set; }

        public async Promise Build()
        {
            if (ProjectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Center, ProjectileType.Value) );
            }

            foreach (var area in Area)
            {
                Offset offset;

                if (Direction != null)
                {
                    if (Direction == Common.Structures.Direction.North)
                    {
                        offset = new Offset(-area.X, -area.Y);
                    }
                    else if (Direction == Common.Structures.Direction.East)
                    {
                        offset = new Offset(area.Y, -area.X);
                    }
                    else if (Direction == Common.Structures.Direction.West)
                    {
                        offset = new Offset(-area.Y, area.X);
                    }
                    else
                    {
                        offset = area;
                    }
                }
                else
                {
                    offset = area;
                }

                Position position = Center.Offset(offset);

                if (MagicEffectType != null)
                {
                    await Context.Current.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.Value) );
                }

                Tile toTile = Context.Current.Server.Map.GetTile(position);

                if (toTile != null)
                {
                    if (OpenTibiaId != null && Count != null)
                    {
                        if (toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                        {

                        }
                        else
                        {
                            await Context.Current.AddCommand(new TileCreateItemCommand(toTile, OpenTibiaId.Value, Count.Value) ).Then( (item) =>
                            {
                                _ = Context.Current.AddCommand(new ItemDecayDestroyCommand(item, 10000) );

                                return Promise.Completed;
                            } );
                        }
                    }

                    foreach (var target in toTile.GetMonsters().Concat<Creature>(toTile.GetPlayers() ).ToList() )
                    {
                        if (Formula != null)
                        {
                            await Context.Current.AddCommand(new CombatAddDamageCommand(Attacker, target, Formula.Formula, Formula.MissedMagicEffectType, Formula.DamageMagicEffectType, Formula.DamageAnimatedTextColor) );

                            if (target.Health == 0)
                            {
                                continue;
                            }
                        }

                        // if (Condition != null)
                        // {
                        //     _ = Context.Current.AddCommand(new CombatAddConditionCommand(target, Condition.SpecialCondition, Condition.MagicEffectType, Condition.AnimatedTextColor, Condition.Damages, Condition.IntervalInMilliseconds) );
                        // 
                        //     if (target.Health == 0)
                        //     {
                        //         continue;
                        //     }
                        // }
                    }
                }
            }
        }
    }
}