using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public class Spell
    {
        public string Name { get; set; }

        public string Group { get; set; }

        public TimeSpan Cooldown { get; set; }

        public TimeSpan GroupCooldown { get; set; }

        public int Level { get; set; }

        public int Mana { get; set; }

        public bool Premium { get; set; }

        public Vocation[] Vocations { get; set; }
    }
}