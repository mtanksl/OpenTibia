using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public class Weapon
    {
        public int Level { get; set; }

        public int Mana { get; set; }

        public Vocation[] Vocations { get; set; }

        public Func<Player, Creature, Item, Promise> Callback { get; set; }
    }
}