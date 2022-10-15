using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatBeamAttackCommand : Command
    {
        public CombatBeamAttackCommand(Creature attacker, Offset[] beam, MagicEffectType magicEffectType, int health)
        {
            Attacker = attacker;

            Beam = beam;

            MagicEffectType = magicEffectType;

            Health = health;
        }

        public Creature Attacker { get; set; }

        public Offset[] Beam { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public int Health { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                foreach (var offset in Beam)
                {
                    Offset directionOffset;

                    if (Attacker.Direction == Direction.North)
                    {
                        directionOffset = new Offset(offset.X, -offset.Y);
                    }
                    else if (Attacker.Direction == Direction.East)
                    {
                        directionOffset = new Offset(offset.Y, offset.X);
                    }
                    else if (Attacker.Direction == Direction.West)
                    {
                        directionOffset = new Offset(-offset.Y, -offset.X);
                    }
                    else
                    {
                        directionOffset = offset;
                    }

                    Position position = Attacker.Tile.Position.Offset(directionOffset);

                    context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType) );

                    Tile tile = context.Server.Map.GetTile(position);

                    if (tile != null)
                    {
                        foreach (var target in tile.GetMonsters().Concat<Creature>(tile.GetPlayers() ).ToList() )
                        {
                            context.AddCommand(new CombatDamageCommand(Attacker, target, Health) );
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}