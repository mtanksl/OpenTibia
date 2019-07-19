using System.Linq; 
using System.Collections.Generic;

namespace OpenTibia.Data
{
    public class PlayerRepository
    {
        private List<AccountRow> accounts = new List<AccountRow>()
        {
            new AccountRow()
            {
                Name = "1",

                Password = "1",

                PremiumDays = 0,

                Players = new List<PlayerRow>()
                {
                    new PlayerRow()
                    {
                       Name = "Player",

                       CoordinateX = 931,

                       CoordinateY = 779,

                       CoordinateZ = 7,

                       World = new WorldRow()
                       {
                           Name = "World",

                           Ip = "192.168.1.11", //Change this to your ip address

                           Port = 7172
                       }
                    }
                }
            }
        };

        public AccountRow GetAccount(string accountName, string accountPassword)
        {
            return accounts.Where(a => a.Name == accountName)
                           .Where(a => a.Password == accountPassword)
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