using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CombatAddConditionCommand : Command
    {
        public CombatAddConditionCommand(Creature creature, SpecialCondition specialCondition, MagicEffectType magicEffectType, AnimatedTextColor animatedTextColor, int[] health, int intervalInMilliseconds)
        {
            Creature = creature;

            SpecialCondition = specialCondition;

            MagicEffectType = magicEffectType;

            AnimatedTextColor = animatedTextColor;

            Health = health;

            IntervalInMilliseconds = intervalInMilliseconds;
        }

        public Creature Creature { get; set; }

        public SpecialCondition SpecialCondition { get; set; }

        private MagicEffectType MagicEffectType;
        
        private AnimatedTextColor AnimatedTextColor;

        public int[] Health { get; set; }

        public int IntervalInMilliseconds { get; set; }

        public override async Promise Execute()
        {
            Player player = Creature as Player;

            if ( !Creature.HasSpecialCondition(SpecialCondition) )
            {
                Creature.AddSpecialCondition(SpecialCondition);

                if (player != null)
                {
                    Context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(Creature.SpecialConditions) );
                }
            }

            for (int i = 0; i < Health.Length; i++)
            {
                if (player != null)
                {
                    Context.Current.AddPacket(player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "You lose " + -Health[i] + " hitpoints.") );
                }

                await Context.Current.AddCommand(new ShowMagicEffectCommand(Creature.Tile.Position, MagicEffectType) );

                await Context.Current.AddCommand(new ShowAnimatedTextCommand(Creature.Tile.Position, AnimatedTextColor, ( -Health[i] ).ToString() ) );

                await Context.Current.AddCommand(new CreatureUpdateHealthCommand(Creature, Creature.Health + Health[i] ) );

                if (i < Health.Length - 1)
                {
                    await Context.Server.Components.AddComponent(Creature, new CreatureSpecialConditionDelayBehaviour(SpecialCondition, IntervalInMilliseconds) ).Promise;
                }
            }

            Creature.RemoveSpecialCondition(SpecialCondition);

            if (player != null)
            {
                Context.AddPacket(player.Client.Connection, new SetSpecialConditionOutgoingPacket(Creature.SpecialConditions) );
            }
        }
    }
}