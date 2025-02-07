using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerBuyNpcTradeEventArgs : GameEventArgs
    {
        public PlayerBuyNpcTradeEventArgs(Player player, ushort openTibiaId, byte type, byte count, int price, bool ignoreCapacity, bool buyWithBackpacks)
        {
            Player = player;

            OpenTibiaId = openTibiaId;

            Type = type;

            Count = count;

            Price = price;

            IgnoreCapacity = ignoreCapacity;

            BuyWithBackpacks = buyWithBackpacks;
        }

        public Player Player { get; }

        public ushort OpenTibiaId { get; }

        public byte Type { get; }

        public byte Count { get; }

        public int Price { get; }

        public bool IgnoreCapacity { get; }

        public bool BuyWithBackpacks { get; }
    }
}