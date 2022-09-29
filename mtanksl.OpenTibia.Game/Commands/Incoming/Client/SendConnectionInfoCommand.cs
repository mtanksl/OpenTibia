using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class SendConnectionInfoCommand : Command
    {
        public SendConnectionInfoCommand(IConnection connection)
        {
            Connection = connection;
        }

        public IConnection Connection { get; set; }

        public override void Execute(Context context)
        {
            context.AddPacket(Connection, new SendConnectionInfoOutgoingPacket() );

            base.Execute(context);
        }
    }
}