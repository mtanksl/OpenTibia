using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class InvisibleCondition : OutfitCondition
    {
        public InvisibleCondition(int durationInMilliseconds) : base(Outfit.Invisible, durationInMilliseconds)
        {

        }
    }
}