using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Plugins
{
    public class Weapon
    {
        public ushort OpenTibiaId { get; set; }

        public int Level { get; set; }

        public int Mana { get; set; }

        public Vocation[] Vocations { get; set; }
    }
}