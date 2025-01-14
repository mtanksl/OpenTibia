using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class NpcSayToPlayerEventArgs : GameEventArgs
    {
        public NpcSayToPlayerEventArgs(Npc npc, Player player, string message)
        {
            Npc = npc;

            Player = player;

            Message = message;
        }

        public Npc Npc { get; }

        public Player Player { get; }

        public string Message { get; }
    }
}