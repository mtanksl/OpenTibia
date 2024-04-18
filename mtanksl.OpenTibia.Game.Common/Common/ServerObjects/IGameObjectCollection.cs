using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGameObjectCollection
    {
        void AddGameObject(GameObject gameObject);

        bool RemoveGameObject(GameObject gameObject);

        Creature GetCreature(uint id);

        Monster GetMonster(uint id);

        Npc GetNpc(uint id);

        Player GetPlayer(uint id);

        IEnumerable<Creature> GetCreatures();

        IEnumerable<Monster> GetMonsters();

        IEnumerable<Npc> GetNpcs();

        IEnumerable<Player> GetPlayers();
    }
}