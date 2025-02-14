using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class ChallengeSpellPlugin : SpellPlugin
    {
        public ChallengeSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            Offset[] area = new Offset[]
            {
                new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
                new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),
            };

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, player.Tile.Position, area, null, MagicEffectType.BlueShimmer, 
                        
                new ChallengeAttack() ) );
        }
    }
}