using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ConditionDto
    {
        public SpecialCondition SpecialCondition { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public AnimatedTextColor? AnimatedTextColor { get; set; }

        public int[] Damages { get; set; }

        public int IntervalInMilliseconds { get; set; }
    }
}