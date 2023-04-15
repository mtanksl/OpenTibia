using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class CreatureThinkBehaviour : Behaviour
    {
        public abstract Promise Update();
    }
}