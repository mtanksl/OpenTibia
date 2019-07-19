using OpenTibia.Common.Structures;
using OpenTibia.Game;

namespace OpenTibia.Common.Objects
{
    public class Client : IClient
    {
        public Client(Server server)
        {
            this.CreatureCollection = new CreatureCollection(server, this);

            this.ContainerCollection = new ContainerCollection(this);
        }

        private Player player;

        public Player Player
        {
            get
            {
                return player;
            }
            set
            {
                if (value != player)
                {
                    var current = player;

                                  player = value;

                    if (value == null)
                    {
                        current.Client = null;
                    }
                    else
                    {
                        player.Client = this;
                    }
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
                if (value != connection)
                {
                    var current = connection;

                                  connection = value;

                    if (value == null)
                    {
                        current.Client = null;
                    }
                    else
                    {
                        connection.Client = this;
                    }
                }
            }
        }

        public FightMode FightMode { get; set; }

        public ChaseMode ChaseMode { get; set; }

        public SafeMode SafeMode { get; set; }

        public ICreatureCollection CreatureCollection { get; }

        public IContainerCollection ContainerCollection { get; }
    }
}