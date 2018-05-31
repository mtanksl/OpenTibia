using OpenTibia.Common.Objects;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class CreatureCollection
    {
        private uint id = 0;

            private Dictionary<uint, Creature> creatures = new Dictionary<uint, Creature>();

        public void AddCreature(Creature creature)
        {
            if (creature.Id == 0)
            {
                creature.Id = ++id;
            }

            creatures.Add(creature.Id, creature);
        }

        public void RemoveCreature(uint creatureId)
        {
            creatures.Remove(creatureId);
        }
        
        public Creature GetCreature(uint creatureId)
        {
            Creature creature;

            creatures.TryGetValue(creatureId, out creature);

            return creature;
        }

        public IEnumerable<Creature> GetCreatures()
        {
            return creatures.Values;
        }

        public IEnumerable<Monster> GetMonsters()
        {
            return creatures.Values.OfType<Monster>();
        }

        public IEnumerable<Npc> GetNpcs()
        {
            return creatures.Values.OfType<Npc>();
        }

        public IEnumerable<Player> GetPlayers()
        {
            return creatures.Values.OfType<Player>();
        }
    }
}