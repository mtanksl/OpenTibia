using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseDebugAssertCommand : IncomingCommand
    {
        public ParseDebugAssertCommand(Player player, string assertLine, string reportDate, string description, string comment)
        {
            Player = player;

            AssertLine = assertLine;

            ReportDate = reportDate;

            Description = description;

            Comment = comment;
        }

        public Player Player { get; set; }

        public string AssertLine { get; set; }

        public string ReportDate { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public override Promise Execute()
        {
            using (var database = Context.Server.DatabaseFactory.Create() )
            {
                database.DebugAssertRepository.AddDebugAssert(new DbDebugAssert()
                {
                    PlayerId = Player.DatabasePlayerId,
                    AssertLine = AssertLine,
                    ReportDate = ReportDate,
                    Description = Description,
                    Comment = Comment,
                    CreationDate = DateTime.UtcNow
                } );

                database.Commit();
            }

            return Promise.Completed;
        }
    }
}