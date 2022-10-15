using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatBeamAttackCommand : Command
    {
        public CombatBeamAttackCommand(Creature attacker, Offset[] beams, MagicEffectType magicEffectType, int health)
        {
            Attacker = attacker;

            Beams = beams;

            MagicEffectType = magicEffectType;

            Health = health;
        }

        public Creature Attacker { get; set; }

        public Offset[] Beams { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public int Health { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
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

                    Position position = Attacker.Tile.Position.Offset(offset);

                    context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType) );

                    Tile tile = context.Server.Map.GetTile(position);

                    if (tile != null)
                    {
                        foreach (var target in tile.GetMonsters().Concat<Creature>(tile.GetPlayers() ).ToList() )
                        {
                            context.AddCommand(new CombatChangeHealthCommand(Attacker, target, Health) );
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}