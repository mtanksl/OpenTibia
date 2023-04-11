using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ShowTextCommand : Command
    {
        public ShowTextCommand(Creature creature, TalkType talkType, string message)
        {
            Creature = creature;

            TalkType = talkType;

            Message = message;
        }

        public Creature Creature { get; set; }

        public TalkType TalkType { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(Creature.Tile.Position) )
                    {
                        Context.AddPacket(observer.Client.Connection, new ShowTextOutgoingPacket(0, Creature.Name, 0, TalkType, Creature.Tile.Position, Message) );
                    }
                }

                resolve();
            } );
        }
    }
}