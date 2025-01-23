using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Houses;
using System.Collections.Generic;
using House = OpenTibia.Common.Objects.House;
using Monster = OpenTibia.Common.Objects.Monster;
using Npc = OpenTibia.Common.Objects.Npc;
using Tile = OpenTibia.Common.Objects.Tile;
using Town = OpenTibia.Common.Objects.Town;
using Waypoint = OpenTibia.Common.Objects.Waypoint;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IMap : IMapGetTile
    {
        ushort Width { get; }

        ushort Height { get; }

        List<string> Warnings { get; }

        void Start(OtbmFile otbmFile, HouseFile houseFile);

        Town GetTown(string name);

        Town GetTown(ushort townId);

        IEnumerable<Town> GetTowns();

        Waypoint GetWaypoint(string name);

        IEnumerable<Waypoint> GetWaypoints();

        House GetHouse(string name);

        House GetHouse(ushort houseId);

        IEnumerable<House> GetHouses();

        // Tile GetTile(Position position);

        IEnumerable<Tile> GetTiles();

        void AddObserver(Position position, Creature creature);

        void RemoveObserver(Position position, Creature creature);

        IEnumerable<Creature> GetObserversOfTypeCreature(Position position);

        IEnumerable<Player> GetObserversOfTypePlayer(Position position);

        IEnumerable<Monster> GetObserversOfTypeMonster(Position position);

        IEnumerable<Npc> GetObserversOfTypeNpc(Position position);
    }
}