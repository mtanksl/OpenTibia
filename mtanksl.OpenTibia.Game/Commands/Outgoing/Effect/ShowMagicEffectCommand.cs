using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ShowMagicEffectCommand : Command
    {
        public ShowMagicEffectCommand(Position position, MagicEffectType magicEffectType)
        {
            Position = position;

            MagicEffectType = magicEffectType;
        }

        public Position Position { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public override Promise Execute()
        {
            ShowMagicEffectOutgoingPacket showMagicEffectOutgoingPacket = new ShowMagicEffectOutgoingPacket(Position, MagicEffectType);

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Position) )
            {
                if (observer.Tile.Position.CanSee(Position) )
                {
                    Context.AddPacket(observer.Client.Connection, showMagicEffectOutgoingPacket);
                }
            }

            return Promise.Completed;
        }
    }
}