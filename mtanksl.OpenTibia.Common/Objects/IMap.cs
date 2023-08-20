using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IMap
    {
        Town GetTown(string name);

        IEnumerable<Town> GetTowns();

        Waypoint GetWaypoint(string name);

        IEnumerable<Waypoint> GetWaypoints();

        Tile GetTile(Position position);

        IEnumerable<Tile> GetTiles();

        void AddObserver(Position position, Creature creature);

        void RemoveObserver(Position position, Creature creature);

        IEnumerable<Creature> GetObserversOfTypeCreature(Position position);

        IEnumerable<Player> GetObserversOfTypePlayer(Position position);

        IEnumerable<Monster> GetObserversTypeOfMonster(Position position);

        IEnumerable<Npc> GetObserversTypeOfNpc(Position position);
    }
}