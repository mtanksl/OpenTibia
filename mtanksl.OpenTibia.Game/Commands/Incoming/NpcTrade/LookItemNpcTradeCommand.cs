using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class LookItemNpcTradeCommand : Command
    {
        public LookItemNpcTradeCommand(Player player, ushort itemId, byte type)
        {
            Player = player;

            ItemId = itemId;

            Type = type;
        }

        public Player Player { get; set; }

        public ushort ItemId { get; set; }

        public byte Type { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {


                resolve(context);
            } );
        }
    }
}