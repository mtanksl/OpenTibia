using System;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public class Rune
    {
        public string Name { get; set; }

        public string Group { get; set; }

        public TimeSpan GroupCooldown { get; set; }

        public int Level { get; set; }

        public int MagicLevel { get; set; }
    }
}