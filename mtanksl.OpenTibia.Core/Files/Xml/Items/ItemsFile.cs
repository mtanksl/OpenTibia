using System.Xml.Linq;
using System.Collections.Generic;

namespace OpenTibia.Xml.Items
{
    public class ItemsFile
    {
        public static ItemsFile Load(string path)
        {
            ItemsFile file = new ItemsFile();

            file.items = new List<Item>();

            foreach ( var itemNode in XElement.Load(path).Elements("item") )
            {
                file.items.Add( Item.Load(itemNode) );
            }
            return file;
        }

        private List<Item> items;

        public List<Item> Items
        {
            get
            {
                return items;
            }
        }
    }
}