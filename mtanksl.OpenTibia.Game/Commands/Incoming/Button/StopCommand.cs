using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class StopCommand : Command
    {
        public StopCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddCommand(new StopWalkCommand(Player) );

                context.AddCommand(new StopAttackCommand(Player) );

                context.AddCommand(new StopFollowCommand(Player) );

                resolve(context);
            } );
        }
    }
}