namespace OpenTibia
{
    public static class FloorChangeExtensions
    {
        public static bool Any(this FloorChange floorChanges, params FloorChange[] values)
        {
            foreach (var floorChange in values)
            {
                if ( (floorChanges & floorChange) == floorChange )
                {
                    return true;
                }
            }

            return false;
        }
    }
}