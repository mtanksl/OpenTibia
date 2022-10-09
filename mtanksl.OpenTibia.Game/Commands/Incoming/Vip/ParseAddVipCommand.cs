using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseAddVipCommand : Command
    {
        public ParseAddVipCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {


                resolve(context);
            } );
        }
    }
}