using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class House
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        public Position Entry { get; set; }

        public Town Town { get; set; }

        public uint Rent { get; set; }

        public uint Size { get; set; }

        public bool Guildhall { get; set; }

        private Dictionary<Position, HouseTile> tiles = new Dictionary<Position, HouseTile>();

        public void AddTile(Position position, HouseTile houseTile)
        {
            tiles.Add(position, houseTile);
        }

        public IEnumerable<HouseTile> GetTiles()
        {
            return tiles.Values;
        }

        public bool CanWalk(string playerName)
        {
            return IsOwner(playerName) || IsSubOwner(playerName) || IsGuest(playerName);
        }

        public bool CanOpenWindow(string playerName)
        {
            return IsOwner(playerName) || IsSubOwner(playerName);
        }

        public bool CanOpenDoor(string playerName, DoorItem doorItem)
        {
            return IsOwner(playerName) || IsSubOwner(playerName) || CanOpenDoor(doorItem.DoorId, playerName);
        }

        private string owner;

        public bool IsOwner(string playerName)
        {
            return owner == playerName;
        }

        private HouseAccessList subOwners = new HouseAccessList();

        public bool IsSubOwner(string playerName)
        {
            return subOwners.Contains(playerName);
        }

        public HouseAccessList GetSubOwnersList()
        {
            return subOwners;
        }

        private HouseAccessList guests = new HouseAccessList();

        public bool IsGuest(string playerName)
        {
            return guests.Contains(playerName);
        }

        public HouseAccessList GetGuestsList()
        {
            return guests;
        }

        private Dictionary<byte, HouseAccessList> doors = new Dictionary<byte, HouseAccessList>();

        public bool CanOpenDoor(byte doorId, string playerName)
        {
            HouseAccessList houseAccessList;

            if (doors.TryGetValue(doorId, out houseAccessList) )
            {
                return houseAccessList.Contains(playerName);
            }

            return false;
        }

        public HouseAccessList GetDoorList(byte doorId)
        {
            HouseAccessList houseAccessList;

            if ( !doors.TryGetValue(doorId, out houseAccessList) )
            {
                houseAccessList = new HouseAccessList();

                doors.Add(doorId, houseAccessList);
            }

            return houseAccessList;
        }
    }
}