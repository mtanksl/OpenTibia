using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MonsterYellCommand : Command
    {
        public MonsterYellCommand(Monster monster, string message)
        {
            Monster = monster;

            Message = message;
        }

        public Monster Monster { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(0, Monster.Name, 0, TalkType.MonsterYell, Monster.Tile.Position, Message);

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Monster.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearYell(Monster.Tile.Position) )
                {
                    Context.AddPacket(observer, showTextOutgoingPacket);
                }
            }

            Context.AddEvent(Monster, new MonsterYellEventArgs(Monster, Message) );

            return Promise.Completed;
        }
    }
}