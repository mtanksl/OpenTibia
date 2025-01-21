using OpenTibia.FileFormats.Xml.Monsters;
using System.IO;
using System.Xml.Linq;

namespace mtanksl.OpenTibia.MonsterImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var fromPath = @"C:\Users\Murilo\Downloads\Crying Damson 0.3.6 860\data\monster";

            var toPath = @"C:\Users\Murilo\Documents\Git\mtanksl.OpenTibia\mtanksl.OpenTibia.GameData\data\monsters";

            // Update all monster .xml files
            /*
            foreach (var folder in Directory.GetDirectories(fromPath) )
            {
                foreach (var file in Directory.GetFiles(folder, "*.xml") )
                {
                    var monsterFile = XElement.Load(file, LoadOptions.PreserveWhitespace);

                    var monster = Monster.Load(monsterFile);

                    var monsterFile2 = XElement.Load(Path.Combine(toPath, monster.Name + ".xml"), LoadOptions.PreserveWhitespace);

                    try
                    {
                        // monsterFile2.Add(new XAttribute("experience", monster.Experience) );

                        var elements = monsterFile.Element("elements");

                        if (elements != null)
                        {
                            monsterFile2.Add(elements);

                            File.WriteAllText(Path.Combine(toPath, monster.Name + ".xml"), "<?xml version=\"1.0\"?>\r\n" + monsterFile2.ToString() );
                        }
                    }
                    catch { }               
                }
            }
            */

            // Copy all monster .xml files
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