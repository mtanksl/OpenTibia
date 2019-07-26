namespace OpenTibia.Network.Packets
{
    public class Offer
    {
        public Offer(ushort itemId, byte type, string name, uint weight, uint buyPrice, uint sellPrice)
        {
            ItemId = itemId;

            Type = type;

            Name = name;

            Weight = weight;

            BuyPrice = buyPrice;

            SellPrice = sellPrice;
        }

        public ushort ItemId { get; set; }

        public byte Type { get; set; }

        public string Name { get; set; }

        public uint Weight { get; set; }

        public uint BuyPrice { get; set; }

        public uint SellPrice { get; set; }
    }
}