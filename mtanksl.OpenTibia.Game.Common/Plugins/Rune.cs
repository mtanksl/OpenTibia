using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Plugins
{
    public class Rune
    {
        public ushort OpenTibiaId { get; set; }

        public string Name { get; set; }

        public string Group { get; set; }

        public TimeSpan GroupCooldown { get; set; }

        public int Level { get; set; }

        public int MagicLevel { get; set; }

        public Vocation[] Vocations { get; set; }
    }
}