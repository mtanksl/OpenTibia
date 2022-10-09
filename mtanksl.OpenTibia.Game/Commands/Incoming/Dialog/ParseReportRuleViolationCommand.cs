using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Incoming;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseReportRuleViolationCommand : Command
    {
        public ParseReportRuleViolationCommand(Player player, ReportRuleViolationIncomingPacket packet)
        {
            Player = player;

            Packet = packet;
        }

        public Player Player { get; set; }

        public ReportRuleViolationIncomingPacket Packet { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {


                resolve(context);
            } );
        }
    }
}