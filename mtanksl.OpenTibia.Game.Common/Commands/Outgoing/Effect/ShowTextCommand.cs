using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ShowTextCommand : Command
    {
        public ShowTextCommand(Creature creature, MessageMode messageMode, string message)
        {
            Creature = creature;

            MessageMode = messageMode;

            Message = message;
        }

        public Creature Creature { get; set; }

        public MessageMode MessageMode { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            ShowTextOutgoingPacket showTextOutgoingPacket = new ShowTextOutgoingPacket(0, Creature.Name, 0, MessageMode, Creature.Tile.Position, Message);

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
            {
                if (observer.Tile.Position.CanHearSay(Creature.Tile.Position) )
                {
                    Context.AddPacket(observer, showTextOutgoingPacket);
                }
            }

            return Promise.Completed;
        }
    }
}