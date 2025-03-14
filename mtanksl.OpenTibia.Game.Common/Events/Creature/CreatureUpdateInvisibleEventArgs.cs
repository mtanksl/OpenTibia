﻿using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class CreatureUpdateInvisibleEventArgs : GameEventArgs
    {
        public CreatureUpdateInvisibleEventArgs(Creature creature, bool invisible)
        {
            Creature = creature;

            Invisible = invisible;
        }

        public Creature Creature { get; }

        public bool Invisible { get; }
    }
}