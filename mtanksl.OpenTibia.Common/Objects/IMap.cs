using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IMap
    {
        Town GetTown(string name);

        Town GetTown(ushort townId);

        IEnumerable<Town> GetTowns();

        Waypoint GetWaypoint(string name);

        IEnumerable<Waypoint> GetWaypoints();

        House GetHouse(string name);

        House GetHouse(ushort houseId);

        IEnumerable<House> GetHouses();

        Tile GetTile(Position position);

        IEnumerable<Tile> GetTiles();

        void AddObserver(Position position, Creature creature);

        void RemoveObserver(Position position, Creature creature);

        IEnumerable<Creature> GetObserversOfTypeCreature(Position position);

        IEnumerable<Player> GetObserversOfTypePlayer(Position position);

        IEnumerable<Monster> GetObserversOfTypeMonster(Position position);

        IEnumerable<Npc> GetObserversOfTypeNpc(Position position);
    }
}