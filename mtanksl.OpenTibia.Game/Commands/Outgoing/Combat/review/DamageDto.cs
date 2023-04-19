using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class DamageDto
    {
        public Func<Creature, Creature, int> Formula { get; set; }

        public MagicEffectType? MissedMagicEffectType { get; set; }

        public MagicEffectType? DamageMagicEffectType { get; set; }

        public AnimatedTextColor? DamageAnimatedTextColor { get; set; }
    }
}