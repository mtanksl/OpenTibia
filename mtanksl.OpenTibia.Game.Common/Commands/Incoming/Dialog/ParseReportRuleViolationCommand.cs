using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
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

        public override async Promise Execute()
        {
            // ctrl + j

            using (var database = Context.Server.DatabaseFactory.Create() )
            {
                Statement statment = null;

                if (Type == 0x01)
                {
                    statment = Context.Server.Channels.GetStatement(StatmentId);
                }

                database.RuleViolationReportRepository.AddRuleViolationReport(new DbRuleViolationReport()
                {
                    PlayerId = Player.DatabasePlayerId,
                    Type = Type,
                    RuleViolation = RuleViolation,
                    Name = Name,
                    Comment = Comment,
                    Translation = Translation,
                    StatmentPlayerId = statment?.DatabasePlayerId,
                    Statment = statment?.Message,
                    StatmentDate = statment?.CreationDate,
                    CreationDate = DateTime.UtcNow
                } );

                await database.Commit();
            }

            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "Your report has been sent.") );

            await Promise.Completed; return;
        }
    }
}