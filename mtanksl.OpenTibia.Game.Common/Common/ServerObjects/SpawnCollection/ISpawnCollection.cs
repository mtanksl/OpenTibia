using OpenTibia.FileFormats.Xml.Spawns;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ISpawnCollection
    {
        void Start(SpawnFile spawnFile);

        HashSet<string> UnknownMonsters { get; }

        HashSet<string> UnknownNpcs { get; }

        IEnumerable<Spawner> GetSpawners();

        void Stop();
    }
}