using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseStopCommand : Command
    {
        public ParseStopCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddCommand(new ParseStopWalkCommand(Player) );

                context.AddCommand(new ParseStopAttackCommand(Player) );

                context.AddCommand(new ParseStopFollowCommand(Player) );

                resolve(context);
            } );
        }
    }
}