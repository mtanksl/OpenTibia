using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class MonsterFile
    {
        public static MonsterFile Load(string path)
        {
            MonsterFile file = new MonsterFile();

            file.monsters = new List<Monster>();

            if (Directory.Exists(path) )
            {
                foreach (var fileName in Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories) )
                {
                    file.monsters.Add( Monster.Load( XElement.Load(fileName) ) );
                }
            }

            return file;
        }

        private List<Monster> monsters;

        public List<Monster> Monsters
        {
            get
            {
                return monsters;
            }
        }
    }
}