using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class AddVipCommand : Command
    {
        public AddVipCommand(Player player, string name)
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