using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatAreaAttackCommand : Command
    {
        public CombatAreaAttackCommand(Creature attacker, Offset[] area, MagicEffectType magicEffectType, int health)
        {
            Attacker = attacker;

            Area = area;

            MagicEffectType = magicEffectType;

            Health = health;
        }

        public Creature Attacker { get; set; }

        public Offset[] Area { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public int Health { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                foreach (var offset in Area)
                {
                    Position position = Attacker.Tile.Position.Offset(offset);

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