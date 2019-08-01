using System.Linq; 
using System.Collections.Generic;

namespace OpenTibia.Data
{
    public class PlayerRepository
    {
        private static readonly string Ip = "192.168.1.11";

        private List<AccountRow> accounts = new List<AccountRow>()
        {
            new AccountRow()
            {
                Name = "",

                Password = "",

                PremiumDays = 0,

                Players = new List<PlayerRow>()
                {
                    new PlayerRow()
                    {
                       Name = "Player 1",

                       CoordinateX = 930,

                       CoordinateY = 779,

                       CoordinateZ = 7,

                       World = new WorldRow()
                       {
                           Name = "World",

                           Ip = Ip,

                           Port = 7172
                       }
                    },

                    new PlayerRow()
                    {
                       Name = "Player 2",

                       CoordinateX = 931,

                       CoordinateY = 779,

                       CoordinateZ = 7,

                       World = new WorldRow()
                       {
                           Name = "World",

                           Ip = Ip,

                           Port = 7172
                       }
                    },

                    new PlayerRow()
                    {
                       Name = "Player 3",

                       CoordinateX = 932,

                       CoordinateY = 779,

                       CoordinateZ = 7,

                       World = new WorldRow()
                       {
                           Name = "World",

                           Ip = Ip,

                           Port = 7172
                       }
                    }
                }
            }
        };

        public AccountRow GetAccount(string accountName, string accountPassword)
        {
            return accounts.Where(a => a.Name == accountName &&
                                       a.Password == accountPassword)
                           .FirstOrDefault();
        }

        public PlayerRow GetPlayer(string accountName, string accountPassword, string playerName)
        {
            AccountRow account = GetAccount(accountName, accountPassword);

            if (account == null)
            {
                return null;
            }

            return account.Players.Where(p => p.Name == playerName)
                                  .FirstOrDefault();
        }
    }
}