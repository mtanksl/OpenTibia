using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerMountCollection
    {
        private HashSet<ushort> mounts = new HashSet<ushort>();

        public bool HasMount(ushort mountId)
        {
            return mounts.Contains(mountId);
        }

        public void SetMount(ushort mountId)
        {
            mounts.Add(mountId);
        }

        public void RemoveMount(ushort mountId)
        {
            mounts.Remove(mountId);
        }

        public IEnumerable<ushort> GetMounts()
        {
            return mounts;
        }
    }
}