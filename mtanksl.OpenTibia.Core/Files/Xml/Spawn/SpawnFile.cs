using System.Xml.Linq;
using System.Collections.Generic;

namespace OpenTibia.Xml.Spawn
{
    public class SpawnFile
    {
        public static SpawnFile Load(string path)
        {
            SpawnFile file = new SpawnFile();

            file.spawns = new List<Spawn>();

            foreach ( var spawnNode in XElement.Load(path).Elements("spawn") )
            {
                file.spawns.Add( Spawn.Load(spawnNode) );
            }
            return file;
        }

        private List<Spawn> spawns;

        public List<Spawn> Spawns
        {
            get
            {
                return spawns;
            }
        }
    }
}