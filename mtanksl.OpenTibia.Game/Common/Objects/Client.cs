using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System.Collections.Generic;

namespace OpenTibia.Common
{
    public class Client : IClient
    {
        private IServer server;

        public Client(IServer server)
        {
            this.server = server;

            this.Battles = new BattleCollection(server, this);

            this.Containers = new ContainerCollection(this);

            this.Windows = new WindowCollection(this);
        }

        private Dictionary<string, object> data;

        public Dictionary<string, object> Data
        {
            get
            {
                return data ?? (data = new Dictionary<string, object>() );
            }
        }
                
        public AccountManagerType AccountManagerType { get; set; }

        public IBattleCollection Battles { get; }

        public IContainerCollection Containers { get; }

        public IWindowCollection Windows { get; }

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

        public IContent GetContent(IContainer container, byte clientIndex)
        {
            if (container is Tile tile)
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

                    if (clientIndex == index)
                    {
                        return _content;
                    }

                    index++;
                }

                return null;
            }

            return container.GetContent(clientIndex);
        }

        public bool TryGetIndex(IContent content, out byte clientIndex)
        {
            switch (content)
            {
                case Item item:

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
                                        clientIndex = index;

                                        return true;
                                    }

                                    index++;
                                }
                            }

                            break;

                        case Inventory inventory:

                            if (Player.Inventory == inventory)
                            {
                                clientIndex = (byte)inventory.GetIndex(content);

                                return true;
                            }

                            break;

                        case Container container:

                            foreach (var pair in Containers.GetIndexedContainers() )
                            {
                                if (pair.Value == container)
                                {
                                    clientIndex = (byte)container.GetIndex(content);

                                    return true;
                                }
                            }

                            break;
                    }

                    break;

                case Creature creature:

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
                                clientIndex = index;

                                return true;
                            }

                            index++;
                        }
                    }

                    break;
            }

            clientIndex = 0;

            return false;
        }

        public SkullIcon GetSkullIcon(Creature creature)
        {
            if (creature is Player observer)
            {
                SkullIcon skullIcon;

                if (server.Combats.SkullContains(observer, out skullIcon) )
                {
                    return skullIcon;
                }

                if (server.Combats.YellowSkullContainsDefense(Player, observer) ) // Was player attacked by observer?
                {
                    return SkullIcon.Yellow;
                }
            }

            return SkullIcon.None;
        }

        public PartyIcon GetPartyIcon(Creature creature)
        {
            if (creature is Player observer)
            {
                Party party = server.Parties.GetPartyByLeader(Player);

                if (party != null)
                {
                    if (observer == party.Leader)
                    {
                        return party.SharedExperienceEnabled ? PartyIcon.YellowSharedExperience : PartyIcon.Yellow;
                    }
                    else
                    {
                        if (party.ContainsMember(observer) )
                        {
                            return party.SharedExperienceEnabled ? PartyIcon.BlueSharedExperience : PartyIcon.Blue;
                        }
                        else if (party.ContainsInvitation(observer) )
                        {
                            return PartyIcon.WhiteBlue;
                        }
                    }
                }
                else
                {
                    party = server.Parties.GetPartyThatContainsMember(Player);

                    if (party != null)
                    {
                        if (observer == party.Leader)
                        {
                            return party.SharedExperienceEnabled ? PartyIcon.YellowSharedExperience : PartyIcon.Yellow;
                        }
                        else
                        {
                            if (party.ContainsMember(observer) )
                            {
                                return party.SharedExperienceEnabled ? PartyIcon.BlueSharedExperience : PartyIcon.Blue;
                            }
                        }
                    }
                    else
                    {
                        foreach (var p in server.Parties.GetPartyThatContainsInvitation(Player) )
                        {
                            if (observer == p.Leader)
                            {
                                return PartyIcon.WhiteYellow;
                            }
                        }
                    }
                }
            }

            return PartyIcon.None;
        }
        
        public WarIcon GetWarIcon(Creature creature)
        {
            //TODO: War

            return WarIcon.None;
        }
    }
}