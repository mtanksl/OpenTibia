using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseReportRuleViolationCommand : IncomingCommand
    {
        public ParseReportRuleViolationCommand(Player player, byte type, byte ruleViolation, string name, string comment, string translation, uint statmentId)
        {
            Player = player;

            Type = type;

            RuleViolation = ruleViolation;

            Name = name;

            Comment = comment;

            Translation = translation;

            StatmentId = statmentId;
        }

        public Player Player { get; set; }

        public byte Type { get; set; }

        public byte RuleViolation { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string Translation { get; set; }

        public uint StatmentId { get; set; }

        public override Promise Execute()
        {
            // ctrl + j

            Context.Database.RuleViolationReportRepository.AddRuleViolationReport(new DbRuleViolationReport()
            {
                PlayerId = Player.DatabasePlayerId,
                Type = Type,
                RuleViolation = RuleViolation,
                Name = Name,
                Comment = Comment,
                Translation = Translation,
                Statment = Type == 0x01 ? Context.Server.Channels.GetStatement(StatmentId).Message : null,
                CreationDate = DateTime.UtcNow
            } );

            Context.Database.Commit();

            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Your report has been sent.") );

            return Promise.Completed;
        }
    }
}