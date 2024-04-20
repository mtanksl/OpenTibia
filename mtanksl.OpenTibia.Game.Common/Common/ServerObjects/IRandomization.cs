namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IRandomization
    {
        bool HasProbability(double probability);

        int Take(int minInclusive, int maxInclusive);

        T Take<T>(T[] array);

        T Take<T>(T[] array, int[] weights);

        T[] Shuffle<T>(T[] array);
    }
}