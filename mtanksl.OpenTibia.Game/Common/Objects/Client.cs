using OpenTibia.Common.Structures;
using OpenTibia.Game;

namespace OpenTibia.Common.Objects
{
    public class Client : IClient
    {
        public Client(Server server)
        {
            this.CreatureCollection = new BattleCollection(server, this);

            this.VipCollection = new VipCollection(this);

            this.ContainerCollection = new ContainerCollection(this);

            this.WindowCollection = new WindowCollection(this);
        }

        public IBattleCollection CreatureCollection { get; }

        public IVipCollection VipCollection { get; }

        public IContainerCollection ContainerCollection { get; }

        public IWindowCollection WindowCollection { get; }

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
    }
}