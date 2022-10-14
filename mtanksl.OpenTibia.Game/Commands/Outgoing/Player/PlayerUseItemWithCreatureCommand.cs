using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerUseItemWithCreatureCommand : Command
    {
        public PlayerUseItemWithCreatureCommand(Player player, Item item, Creature toCreature)
        {
            Player = player;

            Item = item;

            ToCreature = toCreature;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public Creature ToCreature { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );
            } );
        }
    }
}