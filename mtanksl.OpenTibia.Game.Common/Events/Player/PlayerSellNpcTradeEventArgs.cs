using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerSellNpcTradeEventArgs : GameEventArgs
    {
        public PlayerSellNpcTradeEventArgs(Player player, ushort openTibiaId, byte type, byte count, int price, bool keepEquipped) 
        {
            Player = player;

            OpenTibiaId = openTibiaId;

            Type = type;

            Count = count;

            Price = price;

            KeepEquipped = keepEquipped;
        }

        public Player Player { get; }

        public ushort OpenTibiaId { get; }

        public byte Type { get; }

        public byte Count { get; }

        public int Price { get; }

        public bool KeepEquipped { get; }
    }
}