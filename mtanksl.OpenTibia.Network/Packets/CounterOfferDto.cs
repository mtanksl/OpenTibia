namespace OpenTibia.Network.Packets
{
    public class CounterOfferDto
    {
        public CounterOfferDto(ushort itemId, byte count)
        {
            ItemId = itemId;

            Count = count;
        }

        public ushort ItemId { get; set; }

        public byte Count { get; set; }
    }
}