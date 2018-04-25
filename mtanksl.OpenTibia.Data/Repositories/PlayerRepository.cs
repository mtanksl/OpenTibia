using System.Linq; 
using System.Collections.Generic;

namespace OpenTibia.Data
{
    public class PlayerRepository
    {
        private List<Account> accounts = new List<Account>()
        {
            new Account()
            {
                Name = "1",

                Password = "1",

                PremiumDays = 0,

                Players = new List<Player>()
                {
                    new Player()
                    {
                       Name = "Player 1",

                       CoordinateX = 931,

                       CoordinateY = 779,

                       CoordinateZ = 7,

                       World = new World()
                       {
                           Name = "World",

                           Ip = "127.0.0.1", //Change this to your ip address

                           Port = 7172
                       }
                    }
                }
            }
        };

        public Account GetAccount(string accountName, string accountPassword)
        {
            return accounts.Where(a => a.Name == accountName)
                           .Where(a => a.Password == accountPassword)
                           .FirstOrDefault();
        }

        public Player GetPlayer(string accountName, string accountPassword, string playerName)
        {
            Account account = GetAccount(accountName, accountPassword);

            if (account == null)
            {
                return null;
            }

            return account.Players.Where(p => p.Name == playerName)
                                  .FirstOrDefault();
        }
    }
}