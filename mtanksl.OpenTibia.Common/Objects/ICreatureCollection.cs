﻿namespace OpenTibia.Common.Objects
{
    public interface ICreatureCollection
    {
        bool IsKnownCreature(uint creatureId, out uint removeId);
    }
}