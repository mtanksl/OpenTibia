using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class UnjustifiedKill
    {
        public uint TargetId { get; set; }

        public DateTime LastAttack { get; set; }
    }
}