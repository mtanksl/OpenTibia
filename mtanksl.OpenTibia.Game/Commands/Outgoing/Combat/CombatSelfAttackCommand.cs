using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatSelfAttackCommand : Command
    {
        public CombatSelfAttackCommand(Creature target, MagicEffectType magicEffectType, int health)
        {
            Target = target;

            MagicEffectType = magicEffectType;

            Health = health;
        }

        public Creature Target { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public int Health { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddCommand(new ShowMagicEffectCommand(Target.Tile.Position, MagicEffectType) );

                context.AddCommand(new CombatChangeHealthCommand(null, Target, Health) );

                resolve(context);
            } );
        }
    }
}