using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class CloseNpcTradeCommand : Command
    {
        public CloseNpcTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {


                resolve(context);
            } );
        }
    }
}