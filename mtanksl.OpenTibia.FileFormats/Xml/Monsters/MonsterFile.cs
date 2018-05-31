using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class MonsterFile
    {
        public static MonsterFile Load(string path)
        {
            MonsterFile file = new MonsterFile();

            file.monsters = new List<Monster>();

            foreach ( var directoryName in Directory.GetDirectories(path) )
            {
                foreach ( var fileName in Directory.GetFiles(directoryName) )
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