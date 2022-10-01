using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Houses
{
    public class HouseFile
    {
        public static HouseFile Load(string path)
        {
            HouseFile file = new HouseFile();

            file.houses = new List<House>();

            foreach (var houseNode in XElement.Load(path).Elements("house") )
            {
                file.houses.Add( House.Load(houseNode) );
            }

            return file;
        }

        private List<House> houses;

        public List<House> Houses
        {
            get
            {
                return houses;
            }
        }
    }
}