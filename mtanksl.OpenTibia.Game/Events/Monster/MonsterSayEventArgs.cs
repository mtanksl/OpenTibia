using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class MonsterSayEventArgs : GameEventArgs
    {
        public MonsterSayEventArgs(Monster monster, string message)
        {
            Monster = monster;

            Message = message;
        }

        public Monster Monster { get; }

        public string Message { get; }
    }    
}