using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class InvisibleCondition : OutfitCondition
    {
        public InvisibleCondition(TimeSpan duration) : base(Outfit.Invisible, duration)
        {

        }
    }
}