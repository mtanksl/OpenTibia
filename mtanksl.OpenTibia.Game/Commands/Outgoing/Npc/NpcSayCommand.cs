using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class NpcSayCommand : Command
    {
        public NpcSayCommand(Npc npc, string message)
        {
            Npc = npc;

            Message = message;
        }

        public Npc Npc { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Tile.Position.CanHearSay(Npc.Tile.Position) )
                {
                    Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Npc.Name, 0, TalkType.Say, Npc.Tile.Position, Message) );
                }
            }

            return Promise.Completed;
        }
    }
}