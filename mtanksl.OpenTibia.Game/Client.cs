using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game
{
    public class Client : IClient
    {
        private Player player;

        public Player Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;

                if (player != null && player.Client != this)
                {
                    player.Client = this;
                }
            }
        }

        private IConnection connection;

        public IConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;

                if (connection != null && connection.Client != this)
                {
                    connection.Client = this;
                }
            }
        }

        public bool IsKnownCreature(uint creatureId, out uint removeId)
        {
            throw new NotImplementedException();
        }
    }
}