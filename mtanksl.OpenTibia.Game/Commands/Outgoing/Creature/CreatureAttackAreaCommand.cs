using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureAttackAreaCommand : Command
    {
        public CreatureAttackAreaCommand(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack)
        {
            Attacker = attacker;

            Beam = beam;

            Center = center;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            Attack = attack;
        }

        public CreatureAttackAreaCommand(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count)
        {
            Attacker = attacker;

            Beam = beam;

            Center = center;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public CreatureAttackAreaCommand(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, Condition condition)
        {
            Attacker = attacker;

            Beam = beam;

            Center = center;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            OpenTibiaId = openTibiaId;

            Count = count;

            Condition = condition;
        }

        public Creature Attacker { get; set; }

        public bool Beam { get; set; }

        public Position Center { get; set; }

        public Offset[] Area { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public ushort? OpenTibiaId { get; set; }

        public byte? Count { get; set; }

        public Attack Attack { get; set; }

        public Condition Condition { get; set; }

        public override async Promise Execute()
        {
            if (ProjectileType != null)
            {
                await Context.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Center, ProjectileType.Value) );
            }

            foreach (var area in Area)
            {
                Offset offset;

                if (Beam)
                {
                    if (Attacker.Direction == Direction.North)
                    {
                        offset = new Offset(-area.X, -area.Y);
                    }
                    else if (Attacker.Direction == Direction.East)
                    {
                        offset = new Offset(area.Y, -area.X);
                    }
                    else if (Attacker.Direction == Direction.West)
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
                    await Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.Value) );
                }

                Tile toTile = Context.Current.Server.Map.GetTile(position);

                if (toTile != null)
                {
                    if (OpenTibiaId != null)
                    {
                        if ( !toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                        {
                            await Context.AddCommand(new TileCreateItemCommand(toTile, OpenTibiaId.Value, Count.Value) ).Then( (item) =>
                            {
                                _ = Context.AddCommand(new ItemDecayDestroyCommand(item, TimeSpan.FromSeconds(10) ) );

                                return Promise.Completed;
                            } );
                        }
                    }

                    foreach (var monster in toTile.GetMonsters().Concat<Creature>(toTile.GetPlayers().Where(p => p.Vocation != Vocation.Gamemaster) ).ToList() )
                    {
                        if (Attack != null)
                        {
                            await Context.AddCommand(new CreatureAttackCreatureCommand(Attacker, monster, Attack) );
                        }

                        if (Condition != null)
                        {
                            await Context.AddCommand(new CreatureAddConditionCommand(monster, Condition) );
                        }
                    }
                }
            }
        }
    }
}