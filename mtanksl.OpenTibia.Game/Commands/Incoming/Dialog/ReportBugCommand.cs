using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ReportBugCommand : Command
    {
        public ReportBugCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {


                resolve(context);
            } );
        }
    }
}