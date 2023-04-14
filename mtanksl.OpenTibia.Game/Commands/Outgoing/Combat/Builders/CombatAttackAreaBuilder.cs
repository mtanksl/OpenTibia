using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatAttackAreaBuilder
    {
        private Creature attacker;

        public CombatAttackAreaBuilder WithAttacker(Creature attacker)
        {
            this.attacker = attacker;

            return this;
        }
               
        private Position center;

        public CombatAttackAreaBuilder WithCenter(Position center)
        {
            this.center = center;

            return this;
        }

        private Offset[] area;

        private Direction? direction;

        public CombatAttackAreaBuilder WithArea(Offset[] area, Direction? direction)
        {
            this.area = area;

            this.direction = direction;

            return this;
        }

        private ProjectileType? projectileType;

        public CombatAttackAreaBuilder WithProjectileType(ProjectileType? projectileType)
        {
            this.projectileType = projectileType;

            return this;
        }

        private MagicEffectType? magicEffectType;

        public CombatAttackAreaBuilder WithMagicEffectType(MagicEffectType? magicEffectType)
        {
            this.magicEffectType = magicEffectType;

            return this;
        }

        private MagicEffectType? missedMagicEffectType;

        public CombatAttackAreaBuilder WithMissedMagicEffectType(MagicEffectType? missedMagicEffectType)
        {
            this.missedMagicEffectType = missedMagicEffectType;

            return this;
        }

        private MagicEffectType? damageMagicEffectType;

        public CombatAttackAreaBuilder WithDamageMagicEffectType(MagicEffectType? damageMagicEffectType)
        {
            this.damageMagicEffectType = damageMagicEffectType;

            return this;
        }
        
        private AnimatedTextColor? animatedTextColor;

        public CombatAttackAreaBuilder WithAnimatedTextColor(AnimatedTextColor? animatedTextColor)
        {
            this.animatedTextColor = animatedTextColor;

            return this;
        }

        private Func<Creature, Creature, int> formula;

        public CombatAttackAreaBuilder WithFormula(Func<Creature, Creature, int> formula)
        {
            this.formula = formula;

            return this;
        }

        public ushort? openTibiaId { get; set; }

        public byte? count { get; set; }

        public CombatAttackAreaBuilder WithCreateItem(ushort? openTibiaId, byte? count)
        {
            this.openTibiaId = openTibiaId;

            this.count = count;

            return this;
        }

        public async Promise Build()
        {
            if (projectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(attacker.Tile.Position, center, projectileType.Value) );
            }

            foreach (var a in area)
            {
                Offset offset;

                if (direction != null)
                {
                    if (direction == Direction.North)
                    {
                        offset = new Offset(-a.X, -a.Y);
                    }
                    else if (direction == Direction.East)
                    {
                        offset = new Offset(a.Y, -a.X);
                    }
                    else if (direction == Direction.West)
                    {
                        offset = new Offset(-a.Y, a.X);
                    }
                    else
                    {
                        offset = a;
                    }
                }
                else
                {
                    offset = a;
                }

                Position position = center.Offset(offset);

                if (magicEffectType != null)
                {
                    await Context.Current.AddCommand(new ShowMagicEffectCommand(position, magicEffectType.Value) );
                }

                Tile toTile = Context.Current.Server.Map.GetTile(position);

                if (toTile != null)
                {
                    foreach (var target in toTile.GetMonsters().Concat<Creature>(toTile.GetPlayers() ).ToList() )
                    {
                        int damage = formula(attacker, target);

                        if (attacker != target || damage > 0)
                        {
                            if (damage == 0)
                            {
                                if (missedMagicEffectType != null)
                                {
                                    await Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, missedMagicEffectType.Value) );
                                }
                            }
                            else
                            {
                                if (damage < 0)
                                {
                                    if (damageMagicEffectType != null)
                                    {
                                        await Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, damageMagicEffectType.Value) );
                                    }

                                    if (animatedTextColor != null)
                                    {
                                        await Context.Current.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, animatedTextColor.Value, (-damage).ToString() ) );
                                    }
                                }

                                await Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );
                            }

                            if (target is Player player)
                            {
                                if (attacker != null)
                                {
                                    if (damage <= 0)
                                    {
                                        Context.Current.AddPacket(player.Client.Connection, new SetFrameColorOutgoingPacket(attacker.Id, FrameColor.Black) );
                                    }

                                    if (damage < 0)
                                    {
                                        Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints due to an attack by " + attacker.Name + ".") );
                                    }
                                }
                                else
                                {
                                    if (damage < 0)
                                    {
                                        Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -damage + " hitpoints.") );
                                    }
                                }
                            }
                        }
                    }

                    if (openTibiaId != null && count != null)
                    {
                        if (toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                        {

                        }
                        else
                        {
                            var item = await Context.Current.AddCommand(new TileCreateItemCommand(toTile, openTibiaId.Value, count.Value) );
                            
                            Context.Current.AddCommand(new ItemDecayDestroyCommand(item, 10000) );
                        }
                    }
                }
            }
        }
    }
}