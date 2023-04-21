using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

        public Creature Attacker { get; set; }

        public bool Beam { get; set; }

        public Position Center { get; set; }

        public Offset[] Area { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public Attack Attack { get; set; }

        public override async Promise Execute()
        {
            if (ProjectileType != null)
            {
                await Context.Current.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Center, ProjectileType.Value) );
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
                    await Context.Current.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.Value) );
                }

                Tile toTile = Context.Current.Server.Map.GetTile(position);

                if (toTile != null)
                {
                    foreach (var monster in toTile.GetMonsters().ToList() )
                    {
                        await Context.Current.AddCommand(new CreatureAttackCreatureCommand(Attacker, monster, Attack) );
                    }

                    foreach (var player in toTile.GetPlayers().ToList() )
                    {
                        await Context.Current.AddCommand(new CreatureAttackCreatureCommand(Attacker, player, Attack) );
                    }
                }
            }
        }
    }
}