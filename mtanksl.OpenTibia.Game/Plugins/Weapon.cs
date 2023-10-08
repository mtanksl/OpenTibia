using OpenTibia.Common.Structures;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public class Weapon
    {
        public int Level { get; set; }

        public int Mana { get; set; }

        public Vocation[] Vocations { get; set; }
    }
}