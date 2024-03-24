using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MonsterSayCommand : Command
    {
        public MonsterSayCommand(Monster monster, string message)
        {
            Monster = monster;

            Message = message;
        }

        public Monster Monster { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(0, Monster.Name, 0, TalkType.MonsterSay, Monster.Tile.Position, Message);

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Monster.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearSay(Monster.Tile.Position) )
                {
                    Context.AddPacket(observer, showTextOutgoingPacket);

                    Context.AddEvent(observer, new MonsterSayEventArgs(Monster, Message) );
                }
            }

            Context.AddEvent(new MonsterSayEventArgs(Monster, Message) );

            return Promise.Completed;
        }
    }
}