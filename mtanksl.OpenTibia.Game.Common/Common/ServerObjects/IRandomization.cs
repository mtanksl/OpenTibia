namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IRandomization
    {
        int Take(int minInclusive, int maxInclusive);

        T Take<T>(T[] array);

        T Take<T>(T[] array, int[] weights);

        T[] Shuffle<T>(T[] array);
    }
}