using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Items
{
    public class ItemsFile
    {
        public static ItemsFile Load(string path)
        {
            ItemsFile file = new ItemsFile();

            file.items = new List<Item>();

            foreach (var itemNode in XElement.Load(path).Elements("item") )
            {
                file.items.Add( Item.Load(itemNode) );
            }

            return file;
        }
                
        public XDocument Serialize()
        {
            return Serialize(this);
        }

        private static XDocument Serialize(ItemsFile file)
        {
            var xml = new XDocument();

            var itemsNode =
                new XElement("items");

            xml.Add(itemsNode);

            foreach (var item in file.items)
            {
                itemsNode.Add(item.Serialize() );
            }

            return xml;
        }

        public void Save(string path)
        {
            Save(this, path);
        }

        private static void Save(ItemsFile file, string path)
        {
            file.Serialize().Save(path);
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