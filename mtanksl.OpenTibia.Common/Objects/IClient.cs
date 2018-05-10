namespace OpenTibia.Common.Objects
{
    public interface IClient 
    {
        Player Player { get; set; }

        IConnection Connection { get; set; }
        
        bool IsKnownCreature(uint creatureId, out uint removeId);
    }
}