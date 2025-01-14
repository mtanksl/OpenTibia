using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class MonsterYellEventArgs : GameEventArgs
    {
        public MonsterYellEventArgs(Monster monster, string message)
        {
            Monster = monster;

            Message = message;
        }

        public Monster Monster { get; }

        public string Message { get; }
    }    
}