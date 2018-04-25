namespace OpenTibia
{
    public class MagicEffectEvent : GameEvent
    {
        public MagicEffectEvent(Position position, MagicEffectType magicEffectType)
        {
            Position = position;

            MagicEffectType = magicEffectType;
        }

        public Position Position { get; set; }

        public MagicEffectType MagicEffectType { get; set; }
    }
}