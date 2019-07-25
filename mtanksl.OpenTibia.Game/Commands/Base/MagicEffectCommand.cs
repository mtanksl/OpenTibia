using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MagicEffectCommand : Command
    {
        public MagicEffectCommand(MagicEffectType magicEffectType, params Position[] positions)
        {
            Positions = positions;

            MagicEffectType = magicEffectType;
        }

        public Position[] Positions { get; set; }

        public MagicEffectType MagicEffectType { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                foreach (var position in Positions)
                {
                    if (observer.Tile.Position.CanSee(position) )
                    {
                        context.Write(observer.Client.Connection, new ShowMagicEffectOutgoingPacket(position, MagicEffectType) );
                    }
                }
            }

            base.Execute(server, context);
        }
    }
}