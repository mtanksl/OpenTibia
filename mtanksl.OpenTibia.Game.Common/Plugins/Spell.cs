using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Plugins
{
    public class Spell
    {
        public byte? Id { get; set; }

        public string Words { get; set; }

        public string Name { get; set; }

        public string Group { get; set; }

        public TimeSpan Cooldown { get; set; }

        public TimeSpan GroupCooldown { get; set; }

        public int Level { get; set; }

        public int Mana { get; set; }

        public int Soul { get; set; }

        public bool Premium { get; set; }

        public ushort? ConjureOpenTibiaId { get; set; }

        public int? ConjureCount { get; set; }

        public Vocation[] Vocations { get; set; }
    }
}