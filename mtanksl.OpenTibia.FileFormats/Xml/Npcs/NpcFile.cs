using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Npcs
{
    public class NpcFile
    {
        public static NpcFile Load(string path)
        {
            NpcFile file = new NpcFile();

            file.npcs = new List<Npc>();

            if (Directory.Exists(path) )
            {
                foreach (var fileName in Directory.GetFiles(path) )
                {
                    file.npcs.Add( Npc.Load( XElement.Load(fileName) ) );
                }
            }

            return file;
        }

        private List<Npc> npcs;

        public List<Npc> Npcs
        {
            get
            {
                return npcs;
            }
        }
    }
}