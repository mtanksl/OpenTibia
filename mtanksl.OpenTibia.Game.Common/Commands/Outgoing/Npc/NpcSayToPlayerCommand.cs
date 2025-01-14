using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class NpcSayToPlayerCommand : Command
    {
        public NpcSayToPlayerCommand(Npc npc, Player player, string message)
        {
            Npc = npc;

            Player = player;

            Message = message;
        }

        public Npc Npc { get; set; }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(0, Npc.Name, 0, TalkType.PrivateNpcToPlayer, Npc.Tile.Position, Message);

            NpcSayToPlayerEventArgs npcSayToPlayerEventArgs = new NpcSayToPlayerEventArgs(Npc, Player, Message);

            Context.AddPacket(Player, showTextOutgoingPacket);

            Context.AddEvent(Player, npcSayToPlayerEventArgs);

            Context.AddEvent(npcSayToPlayerEventArgs);

            return Promise.Completed;
        }
    }
}