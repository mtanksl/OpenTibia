using OpenTibia.FileFormats.Xml.Monsters;
using System.IO;
using System.Xml.Linq;

namespace OpenTibia.MonsterImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var fromPath = @"";

            var toPath = @"";

            /*
            foreach (var folder in Directory.GetDirectories(fromPath) )
            {
                foreach (var file in Directory.GetFiles(folder, "*.xml") )
                {
                    var monsterFile = Monster.Load(XElement.Load(file) );

                    var monsterFile2 = XElement.Load(Path.Combine(toPath, monsterFile.Name + ".xml"), LoadOptions.PreserveWhitespace);

                    try
                    {
                        monsterFile2.Add(new XAttribute("experience", monsterFile.Experience));

                        File.WriteAllText(Path.Combine(toPath, monsterFile.Name + ".xml"), "<?xml version=\"1.0\"?>\r\n" + monsterFile2.ToString() );
                    }
                    catch { }               
                }
            }
            */

            /*
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
            */
        }
    }
}