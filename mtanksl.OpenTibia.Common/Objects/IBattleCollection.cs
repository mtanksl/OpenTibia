namespace OpenTibia.Common.Objects
{
    public interface IBattleCollection
    {
        bool IsKnownCreature(uint creatureId, out uint removeId);
    }
}