using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public class Ammunition
    {
        public Func<Player, Creature, Item, Item, Promise> Callback { get; set; }
    }
}