using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatBeamAttackCommand : Command
    {
        public CombatBeamAttackCommand(Creature attacker, Offset[] beams, MagicEffectType? magicEffectType, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Beams = beams;

            MagicEffectType = magicEffectType;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Offset[] Beams { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

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

                    if (MagicEffectType != null)
                    {
                        context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.Value) );
                    }

                    Tile tile = context.Server.Map.GetTile(position);

                    if (tile != null)
                    {
                        foreach (var target in tile.GetMonsters().Concat<Creature>(tile.GetPlayers() ).ToList() )
                        {
                            int health = Formula(Attacker, target);

                            if (target != Attacker || health > 0)
                            {
                                context.AddCommand(new CombatChangeHealthCommand(Attacker, target, health) );
                            }
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}