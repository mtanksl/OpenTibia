using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Data
{
    public class PlayerRepository
    {
        private string gameServerIpAddress;

        private int gameServerPort;

        public PlayerRepository(string gameServerIpAddress, int gameServerPort)
        {
            this.gameServerIpAddress = gameServerIpAddress;

            this.gameServerPort = gameServerPort;
        }

        private List<AccountRow> accounts;

        public AccountRow GetAccount(string accountName, string accountPassword)
        {
            if (accounts == null)
            {
                accounts = new List<AccountRow>()
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

                                   Ip = gameServerIpAddress,

                                   Port = gameServerPort
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

                                   Ip = gameServerIpAddress,

                                   Port = gameServerPort
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

                                   Ip = gameServerIpAddress,

                                   Port = gameServerPort
                               }
                            }
                        }
                    }
                };
            }

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