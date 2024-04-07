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

        public string Owner { get; set; }

        public bool IsOwner(string playerName)
        {
            return Owner == playerName;
        }

        private HouseAccessList subOwnersList = new HouseAccessList();

        public bool IsSubOwner(string playerName)
        {
            return subOwnersList.Contains(playerName);
        }

        public HouseAccessList GetSubOwnersList()
        {
            return subOwnersList;
        }

        private HouseAccessList guestsList = new HouseAccessList();

        public bool IsGuest(string playerName)
        {
            return guestsList.Contains(playerName);
        }

        public HouseAccessList GetGuestsList()
        {
            return guestsList;
        }

        private Dictionary<byte, HouseAccessList> doorsList = new Dictionary<byte, HouseAccessList>();

        public bool CanOpenDoor(byte doorId, string playerName)
        {
            HouseAccessList houseAccessList;

            if (doorsList.TryGetValue(doorId, out houseAccessList) )
            {
                return houseAccessList.Contains(playerName);
            }

            return false;
        }

        public HouseAccessList GetDoorList(byte doorId)
        {
            HouseAccessList houseAccessList;

            if ( !doorsList.TryGetValue(doorId, out houseAccessList) )
            {
                houseAccessList = new HouseAccessList();

                doorsList.Add(doorId, houseAccessList);
            }

            return houseAccessList;
        }

        public IEnumerable< KeyValuePair<byte, HouseAccessList> > GetDoorsList()
        {
            return doorsList;
        }
    }
}