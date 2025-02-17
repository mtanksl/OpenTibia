﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class CreatureStepInPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnSteppingIn(Creature creature, Tile toTile);

        public abstract Promise OnStepIn(Creature creature, Tile fromTile, Tile toTile);
    }
}