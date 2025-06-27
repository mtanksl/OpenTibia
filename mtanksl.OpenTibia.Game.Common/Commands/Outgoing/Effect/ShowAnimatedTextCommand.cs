using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ShowAnimatedTextCommand : Command
    {
        public ShowAnimatedTextCommand(IContent content, AnimatedTextColor animatedTextColor, uint value)
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

            Value = value;
        }

        public ShowAnimatedTextCommand(Position position, AnimatedTextColor animatedTextColor, uint value)
        {
            Position = position;

            AnimatedTextColor = animatedTextColor;

            Value = value;
        }

        public Position Position { get; set; }

        public AnimatedTextColor AnimatedTextColor { get; set; }

        public uint Value { get; set; }

        public override Promise Execute()
        {
            if (Position != null)
            {
                IOutgoingPacket showAnimatedTextOutgoingPacket;

                if ( !Context.Server.Features.HasFeatureFlag(FeatureFlag.ConsoleMessageOtherCreatures) )
                {  
                    showAnimatedTextOutgoingPacket = new ShowAnimatedTextOutgoingPacket(Position, AnimatedTextColor, Value.ToString() );
                }
                else
                {
                    if (AnimatedTextColor == AnimatedTextColor.White)
                    {
                        showAnimatedTextOutgoingPacket = new ShowWindowTextOutgoingPacket(MessageMode.Exp, Position, Value, AnimatedTextColor, Value.ToString() );
                    }
                    else
                    {
                        showAnimatedTextOutgoingPacket = new ShowWindowTextOutgoingPacket(MessageMode.DamageOthers, Position, Value, AnimatedTextColor, 0, AnimatedTextColor, Value.ToString() );
                    }
                }

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