using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseReportBugCommand : Command
    {
        public ParseReportBugCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // ctrl + z

            if (Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster)
            {
                Context.Database.BugReportRepository.AddBugReport(new DbBugReport()
                {
                    PlayerId = Player.DatabasePlayerId,
                    Message = Message,
                    CreationDate = DateTime.UtcNow
                } );

                Context.Database.Commit();

                Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Your report has been sent.") );

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}