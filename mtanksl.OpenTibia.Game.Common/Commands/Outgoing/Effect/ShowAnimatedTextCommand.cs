using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ShowAnimatedTextCommand : Command
    {
        public ShowAnimatedTextCommand(IContent content, AnimatedTextColor animatedTextColor, string message)
        {
            Position position = null;

            switch (content)
            {
                case Item item:

                    switch (item.Root() )
                    {
                        case Tile tile:

                            position = tile.Position;

                            break;

                        case Inventory inventory:

                            position = inventory.Player.Tile.Position;

                            break;

                        case Safe safe:

                            position = safe.Player.Tile.Position;

                            break;
                    }

                    break;

                case Creature creature:

                    position = creature.Tile.Position;

                    break;
            }

            Position = position;

            AnimatedTextColor = animatedTextColor;

            Message = message;
        }

        public ShowAnimatedTextCommand(Position position, AnimatedTextColor animatedTextColor, string message)
        {
            Position = position;

            AnimatedTextColor = animatedTextColor;

            Message = message;
        }

        public Position Position { get; set; }

        public AnimatedTextColor AnimatedTextColor { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            if (Position != null)
            {
                ShowAnimatedTextOutgoingPacket showAnimatedTextOutgoingPacket = new ShowAnimatedTextOutgoingPacket(Position, AnimatedTextColor, Message);

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Position) )
                {
                    if (observer.Tile.Position.CanHearSay(Position) )
                    {
                        Context.AddPacket(observer, showAnimatedTextOutgoingPacket);
                    }
                }
            }

            return Promise.Completed;
        }
    }
}