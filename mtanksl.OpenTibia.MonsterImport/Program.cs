using OpenTibia.FileFormats.Xml.Monsters;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace mtanksl.OpenTibia.MonsterImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var fromPath = @"";

            var toPath = @"";

            // Update all monster .xml files
            /*
            HashSet<string> names = new HashSet<string>();

            foreach (var folder in Directory.GetDirectories(fromPath) )
            {
                foreach (var file in Directory.GetFiles(folder, "*.xml") )
                {
                    var monsterFile = XElement.Load(file, LoadOptions.PreserveWhitespace);

                    var monster = Monster.Load(monsterFile);

                    if ( !names.Add(monster.Name) )
                    {
                        continue;
                    }

                    var monsterFile2 = XElement.Load(Path.Combine(toPath, monster.Name + ".xml"), LoadOptions.PreserveWhitespace);

                    try
                    {
                        // monsterFile2.Add(new XAttribute("experience", monster.Experience) );

                        // var elements = monsterFile.Element("elements");
                        // 
                        // if (elements != null)
                        // {
                        //     monsterFile2.Add(elements);
                        // }

                        // monsterFile2.Add(new XAttribute("race", monster.Race.ToString().ToLower() ) );

                        File.WriteAllText(Path.Combine(toPath, monster.Name + ".xml"), "<?xml version=\"1.0\"?>\r\n" + monsterFile2.ToString() );
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