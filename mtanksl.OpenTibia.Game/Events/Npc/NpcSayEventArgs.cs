using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class NpcSayEventArgs : GameEventArgs
    {
        public NpcSayEventArgs(Npc npc, string message)
        {
            Npc = npc;

            Message = message;
        }

        public Npc Npc { get; }

        public string Message { get; }
    }
}