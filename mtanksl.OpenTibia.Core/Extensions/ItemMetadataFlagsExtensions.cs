namespace OpenTibia
{
    public static class ItemMetadataFlagsExtensions
    {
        public static bool Any(this ItemMetadataFlags flags, ItemMetadataFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}