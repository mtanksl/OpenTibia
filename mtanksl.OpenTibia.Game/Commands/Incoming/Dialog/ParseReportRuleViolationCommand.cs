using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
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

        public override Promise Execute()
        {
            // ctrl + j

            Context.Database.RuleViolationReportRepository.AddRuleViolationReport(new DbRuleViolationReport()
            {
                PlayerId = Player.DatabasePlayerId,
                Name = Packet.Name,
                Comment = Packet.Comment,
                Translation = Packet.Translation,
                CreationDate = DateTime.UtcNow
            } );

            Context.Database.Commit();

            Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Your report has been sent.") );

            return Promise.Completed;
        }
    }
}