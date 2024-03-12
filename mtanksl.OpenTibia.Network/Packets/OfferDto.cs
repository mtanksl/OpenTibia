namespace OpenTibia.Network.Packets
{
    public class OfferDto
    {
        public OfferDto(ushort tibiaId, byte type, string name, uint weight, uint buyPrice, uint sellPrice)
        {
            TibiaId = tibiaId;

            Type = type;

            Name = name;

            Weight = weight;

            BuyPrice = buyPrice;

            SellPrice = sellPrice;
        }

        public ushort TibiaId { get; set; }

        public byte Type { get; set; }

        public string Name { get; set; }

        public uint Weight { get; set; }

        public uint BuyPrice { get; set; }

        public uint SellPrice { get; set; }
    }
}