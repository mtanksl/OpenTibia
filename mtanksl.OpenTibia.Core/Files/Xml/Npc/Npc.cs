using System.Xml.Linq;

namespace OpenTibia.Xml.Npc
{
    public class Npc
    {
        public static Npc Load(XElement npcNode)
        {
            Npc npc = new Npc();

            npc.Name = (string)npcNode.Attribute("name");

            npc.Speed = (ushort)(uint)npcNode.Attribute("speed");

            XElement healthNode = npcNode.Element("health");

            npc.Health = (ushort)(uint)healthNode.Attribute("now");

            npc.MaxHealth = (ushort)(uint)healthNode.Attribute("max");

            XElement outfitNode = npcNode.Element("look");

            npc.Outfit = new Outfit( (int)outfitNode.Attribute("type"), (int)outfitNode.Attribute("head"), (int)outfitNode.Attribute("body"), (int)outfitNode.Attribute("legs"), (int)outfitNode.Attribute("feet"), Addons.None );

            return npc;
        }

        public string Name { get; set; }

        public ushort Speed { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public Outfit Outfit { get; set; }        
    }
}