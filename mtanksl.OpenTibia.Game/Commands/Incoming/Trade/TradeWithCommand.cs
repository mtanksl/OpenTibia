using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public abstract class TradeWithCommand : Command
    {
        public TradeWithCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
    }
}