using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ConditionInvisible : ConditionOutfit
    {
        public ConditionInvisible(int durationInMilliseconds) : base(Outfit.Invisible, durationInMilliseconds)
        {

        }
    }
}