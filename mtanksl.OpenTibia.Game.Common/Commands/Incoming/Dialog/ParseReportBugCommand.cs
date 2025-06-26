using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseReportBugCommand : IncomingCommand
    {
        public ParseReportBugCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override async Promise Execute()
        {
            // ctrl + z

            if (Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster)
            {
                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    database.BugReportRepository.AddBugReport(new DbBugReport()
                    {
                        PlayerId = Player.DatabasePlayerId,
                        PositionX = Player.Tile.Position.X,
                        PositionY = Player.Tile.Position.Y,
                        PositionZ = Player.Tile.Position.Z,
                        Message = Message,
                        CreationDate = DateTime.UtcNow
                    } );

                    await database.Commit();
                }

                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "Your report has been sent.") );

                await Promise.Completed; return;
            }

            await Promise.Break; return;
        }
    }
}