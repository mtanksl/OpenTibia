using System.Xml.Linq;
using System.Collections.Generic;

namespace OpenTibia.Xml.House
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