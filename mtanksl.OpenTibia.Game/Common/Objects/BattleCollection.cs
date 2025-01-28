using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common
{
    public class BattleCollection : IBattleCollection
    {
        private IServer server;

        private IClient client;

        public BattleCollection(IServer server, IClient client)
        {
            this.server = server;

            this.client = client;
        }

        private HashSet<uint> creatureIds = new HashSet<uint>();

        public bool IsKnownCreature(uint creatureId, out uint removeId)
        {
            if ( creatureIds.Add(creatureId) )
            {
                if (creatureIds.Count > Constants.CreaturesPerClient)
                {
                    removeId = creatureIds.Where(id =>
                    {
                        if (id != client.Player.Id)
                        {
                            Creature creature = server.GameObjects.GetCreature(id);

                            if (creature == null)
                            {
                                return true;
                            }

                            byte clientIndex;

                            if ( !client.TryGetIndex(creature, out clientIndex) )
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
                            if (id != client.Player.Id)
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
    }
}