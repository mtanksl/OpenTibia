using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureAttackAreaCommand : Command
    {     
        public CreatureAttackAreaCommand(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack)
      
            : this(attacker, beam, center, area, projectileType, magicEffectType, attack, null)
        {

        }

        public CreatureAttackAreaCommand(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack, Condition condition)
        {
            Attacker = attacker;

            Beam = beam;

            Center = center;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            Attack = attack;

            Condition = condition;
        }

        public CreatureAttackAreaCommand(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count)

            : this(attacker, beam, center, area, projectileType, magicEffectType, openTibiaId, count, null, null)
        {
           
        }

        public CreatureAttackAreaCommand(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, Attack attack)

            : this(attacker, beam, center, area, projectileType, magicEffectType, openTibiaId, count, attack, null) 
        {
        
        }

        public CreatureAttackAreaCommand(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, Attack attack, Condition condition)
        {
            Attacker = attacker;

            Beam = beam;

            Center = center;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;
                        
            OpenTibiaId = openTibiaId;

            Count = count;

            Attack = attack;

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
                await Context.AddCommand(new ShowProjectileCommand(Attacker, Center, ProjectileType.Value) );
            }

            bool hit = false;

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

                Tile toTile = Context.Server.Map.GetTile(Center.Offset(offset) );

                if (toTile != null)
                {
                    if (toTile.NotWalkable || toTile.ProtectionZone || !Context.Server.Pathfinding.CanThrow(Center, toTile.Position) )
                    {

                    }
                    else
                    {
                        if (MagicEffectType != null)
                        {
                            await Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Value) );
                        }
                        
                        if (OpenTibiaId != null)
                        {
                            var item = await Context.AddCommand(new TileCreateItemCommand(toTile, OpenTibiaId.Value, Count.Value) );
                            
                                   _ = Context.AddCommand(new ItemDecayDestroyCommand(item, TimeSpan.FromSeconds(10) ) );
                        }

                        if (Attack != null || Condition != null)
                        {
                            if (Attacker is Monster)
                            {
                                foreach (var player in toTile.GetPlayers().Where(p => p.Rank != Rank.Gamemaster && p.Rank != Rank.AccountManager).ToArray() )
                                {
                                    await Context.AddCommand(new CreatureAttackCreatureCommand(Attacker, player, Attack, Condition) );
                                
                                    hit = true;
                                }
                            }
                            
                            if (Attacker is Player attacker && Context.Server.Config.GameplayWorldType != WorldType.NonPvp && attacker.Level > Context.Server.Config.GameplayProtectionLevel)
                            {
                                foreach (var player in toTile.GetPlayers().Where(p => p.Rank != Rank.Gamemaster && p.Rank != Rank.AccountManager && p.Level > Context.Server.Config.GameplayProtectionLevel).ToArray() )
                                {
                                    await Context.AddCommand(new CreatureAttackCreatureCommand(Attacker, player, Attack, Condition) );
                               
                                    hit = true;
                                }
                            }

                            foreach (var monster in toTile.GetMonsters().ToArray() )
                            {
                                await Context.AddCommand(new CreatureAttackCreatureCommand(Attacker, monster, Attack, Condition) );

                                hit = true;
                            }
                        }
                    }
                }
            }

            {
                if ( !hit && Attack is DamageAttack && Attacker is Player attacker)
                {
                    await Context.AddCommand(new CreatureAddConditionCommand(attacker, new LogoutBlockCondition(TimeSpan.FromSeconds(Context.Server.Config.GameplayLogoutBlockSeconds) ) ) );
                }
            }
        }
    }
}