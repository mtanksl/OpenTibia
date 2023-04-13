using OpenTibia.FileFormats.Xml.Monsters;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OpenTibia.MonsterImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var fromPath = @"";

            var toPath = @"";

            var serializer = new XmlSerializer(typeof(Monster) );

            var ns = new XmlSerializerNamespaces();
            
            ns.Add("", "");

            foreach (var folder in Directory.GetDirectories(fromPath) )
            {
                foreach (var file in Directory.GetFiles(folder, "*.xml") )
                {
                    var monsterFile = Monster.Load(XElement.Load(file) );

                    using (var destination = File.Create(Path.Combine(toPath, monsterFile.Name + ".xml") ) )
                    {
                        serializer.Serialize(destination, monsterFile, ns);
                    } 
                }
            }
        }
    }
}