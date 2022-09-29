using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TextCommand : Command
    {
        public TextCommand(Creature creature, TalkType talkType, string message)
        {
            Creature = creature;

            TalkType = talkType;

            Message = message;
        }

        public Creature Creature { get; set; }

        public TalkType TalkType { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Creature.Tile.Position) )
                {
                    context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Creature.Name, 0, TalkType, Creature.Tile.Position, Message) );
                }
            }

            base.Execute(context);
        }
    }
}