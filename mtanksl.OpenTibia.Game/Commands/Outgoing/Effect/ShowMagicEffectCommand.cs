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

        public override void Execute(Context context)
        {
            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Position) )
                {
                    context.AddPacket(observer.Client.Connection, new ShowMagicEffectOutgoingPacket(Position, MagicEffectType) );
                }
            }

            OnComplete(context);
        }
    }
}