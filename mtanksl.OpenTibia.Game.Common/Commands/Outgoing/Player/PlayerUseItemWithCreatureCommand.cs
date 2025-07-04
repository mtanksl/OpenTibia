﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

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

        public override Promise Execute()
        {
            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouCanNotUseThisObject) );

            return Promise.Break;
        }
    }
}