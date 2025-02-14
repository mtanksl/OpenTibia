using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Spells
{
    public class CreatureIllusionSpellPlugin : SpellPlugin
    {
        public CreatureIllusionSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            if (parameter != null)
            {
                MonsterMetadata monsterMetadata = Context.Server.MonsterFactory.GetMonsterMetadata(parameter);

                if (monsterMetadata != null && monsterMetadata.Illusionable)
                {
                    return Promise.FromResultAsBooleanTrue;
                }
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            MonsterMetadata monsterMetadata = Context.Server.MonsterFactory.GetMonsterMetadata(parameter);

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(player, 
                            
                    new OutfitCondition(monsterMetadata.Outfit, new TimeSpan(0, 3, 0) ) ) );
            } );
        }
    }
}