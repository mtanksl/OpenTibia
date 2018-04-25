namespace OpenTibia
{
    public class Offer
    {
        public ushort ItemId { get; set; }

        public byte Type { get; set; }

        public string Name { get; set; }

        public uint Weight { get; set; }

        public uint BuyPrice { get; set; }

        public uint SellPrice { get; set; }
    }
}