using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseQuestsCommand : Command
    {
        public ParseQuestsCommand(Player player)
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