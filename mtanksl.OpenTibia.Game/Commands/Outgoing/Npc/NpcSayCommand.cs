using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
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
            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(0, Npc.Name, 0, TalkType.Say, Npc.Tile.Position, Message);

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Npc.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearSay(Npc.Tile.Position) )
                {                        
                    Context.AddPacket(observer, showTextOutgoingPacket);
                   
                    Context.AddEvent(observer, new NpcSayEventArgs(Npc, Message) );
                }
            }

            Context.AddEvent(new NpcSayEventArgs(Npc, Message) );

            return Promise.Completed;
        }
    }
}