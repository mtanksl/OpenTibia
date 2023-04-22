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

        public bool TryGetIndex(IContent content, out byte _index)
        {
            if (content is Item item)
            {
                switch (item.Parent)
                {
                    case Tile tile:

                        if (Player.Tile.Position.CanSee(tile.Position) )
                        {
                            byte index = 0;

                            foreach (var _content in tile.GetContents() )
                            {
                                if (index >= Constants.ObjectsPerPoint)
                                {
                                    break;
                                }

                                if (_content is Creature _creature && _creature != Player && _creature.Invisible)
                                {
                                    continue;
                                }

                                if (_content == item)
                                {
                                    _index = index;

                                    return true;
                                }

                                index++;
                            }
                        }

                        break;

                    case Inventory inventory:

                        if (Player.Inventory == inventory)
                        {
                            _index = inventory.GetIndex(content);

                            return true;
                        }

                        break;

                    case Container container:

                        foreach (var pair in ContainerCollection.GetIndexedContainers() )
                        {
                            if (pair.Value == container)
                            {
                                _index = container.GetIndex(content);

                                return true;
                            }
                        }

                        break;
                }
            }
            else if (content is Creature creature)
            {
                if (Player.Tile.Position.CanSee(creature.Tile.Position) )
                {
                    byte index = 0;

                    foreach (var _content in creature.Tile.GetContents() )
                    {
                        if (index >= Constants.ObjectsPerPoint)
                        {
                            break;
                        }

                        if (_content is Creature _creature && _creature != Player && _creature.Invisible)
                        {
                            continue;
                        }

                        if (_content == creature)
                        {
                            _index = index;

                            return true;
                        }

                        index++;
                    }
                }
            }

            _index = 0;

            return false;
        }
    }
}