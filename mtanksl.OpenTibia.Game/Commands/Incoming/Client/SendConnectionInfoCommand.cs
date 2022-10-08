using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class SendConnectionInfoCommand : Command
    {
        public SendConnectionInfoCommand(IConnection connection)
        {
            Connection = connection;
        }

        public IConnection Connection { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddPacket(Connection, new SendConnectionInfoOutgoingPacket() );

                resolve(context);
            } );
        }
    }
}