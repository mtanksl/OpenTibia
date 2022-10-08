using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class LookItemTradeCommand : Command
    {
        public LookItemTradeCommand(Player player, byte windowId, byte index)
        {
            WindowId = windowId;

            Index = index;
        }

        public Player Player { get; set; }

        public byte WindowId { get; set; }

        public byte Index { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {


                resolve(context);
            } );
        }
    }
}