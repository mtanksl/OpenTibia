using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class AnimatedTextCommand : Command
    {
        public AnimatedTextCommand(Position position, AnimatedTextColor animatedTextColor, string message)
        {
            Position = position;

            AnimatedTextColor = animatedTextColor;

            Message = message;
        }

        public Position Position { get; set; }

        public AnimatedTextColor AnimatedTextColor { get; set; }

        public string Message { get; set; }

        public override void Execute(Context context)
        {
            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Position) )
                {
                    context.WritePacket(observer.Client.Connection, new ShowAnimatedTextOutgoingPacket(Position, AnimatedTextColor, Message) );
                }
            }

            base.OnCompleted(context);
        }
    }
}