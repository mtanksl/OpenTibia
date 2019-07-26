namespace OpenTibia.Network.Packets
{
    public class CounterOffer
    {
        public CounterOffer(ushort itemId, byte count)
        {
            ItemId = itemId;

            Count = count;
        }

        public ushort ItemId { get; set; }

        public byte Count { get; set; }
    }
}