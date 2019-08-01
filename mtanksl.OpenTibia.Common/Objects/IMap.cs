using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IMap
    {
        Tile GetTile(Position position);

        IEnumerable<Tile> GetTiles();

        void AddCreature(Creature creature);

        void RemoveCreature(Creature creature);

        Creature GetCreature(uint creatureId);

        IEnumerable<Creature> GetCreatures();

        IEnumerable<Monster> GetMonsters();

        IEnumerable<Npc> GetNpcs();

        IEnumerable<Player> GetPlayers();
    }
}