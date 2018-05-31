using OpenTibia.Common.Objects;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class Client : IClient
    {
        private Server server;

        public Client(Server server)
        {
            this.server = server;
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

        private HashSet<uint> creatureIds = new HashSet<uint>();

        public bool IsKnownCreature(uint creatureId, out uint removeId)
        {
            if ( creatureIds.Add(creatureId) )
            {
                if (creatureIds.Count > 250)
                {
                    removeId = creatureIds.Where(id =>
                    {
                        if (id != player.Id)
                        {
                            Creature creature = server.CreatureCollection.GetCreature(id);

                            if (creature == null || !player.Tile.Position.CanSee(creature.Tile.Position) )
                            {
                                return true;
                            }
                        }

                        return false;

                    } ) .FirstOrDefault();

                    if (removeId == 0)
                    {
                        removeId = creatureIds.Where(id =>
                        {
                            if (id != player.Id)
                            {
                                return true;
                            }

                            return false;

                        } ).First();
                    }

                    creatureIds.Remove(removeId);

                    return false;
                }

                removeId = 0;

                return false;
            }

            removeId = 0;

            return true;
        }

        public SchedulerEvent Walking { get; set; }
    }
}