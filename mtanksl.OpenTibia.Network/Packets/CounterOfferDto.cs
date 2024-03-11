namespace OpenTibia.Network.Packets
{
    public class CounterOfferDto
    {
        public CounterOfferDto(ushort tibiaId, byte count)
        {
            TibiaId = tibiaId;

            Count = count;
        }

        public ushort TibiaId { get; set; }

        public byte Count { get; set; }
    }
}