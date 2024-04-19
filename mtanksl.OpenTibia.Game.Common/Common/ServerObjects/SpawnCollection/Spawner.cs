using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Spawner
    {
        public uint SpawnTime { get; set; }

        public Tile Tile { get; set; }

        public Monster Monster { get; set; }

        public DateTime? NextSpawn { get; set; }
    }
}