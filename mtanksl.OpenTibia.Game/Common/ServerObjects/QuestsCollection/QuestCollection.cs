using NLua;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class QuestCollection : IDisposable
    {
        private Server server;

        public QuestCollection(Server server)
        {
            this.server = server;
        }

        LuaScope script;

        public void Start()
        {
            script = server.LuaScripts.Create(server.PathResolver.GetFullPath("data/quests/config.lua") );

            foreach (LuaTable lQuest in ( (LuaTable)script["quests"] ).Values)
            {
                Quest quest = new Quest()
                {
                    Id = (ushort)(long)lQuest["id"],

                    Name = (string)lQuest["name"]
                };

                foreach (LuaTable lMission in ( (LuaTable)lQuest["missions"] ).Values)
                {
                    Mission mission = new Mission()
                    {
                        Name = (string)lMission["name"],

                        Description = (string)lMission["description"],

                        StorageKey = (int)(long)lMission["storagekey"],

                        StorageValue = (int)(long)lMission["storagevalue"]
                    };

                    quest.Missions.Add(mission);
                }

                quests.Add(quest.Id, quest);
            }
        }

        private Dictionary<ushort, Quest> quests = new Dictionary<ushort, Quest>();

        public Quest GetQuestById(ushort id)
        {
            Quest quest;

            if (quests.TryGetValue(id, out quest) )
            {
                return quest;
            }

            return null;
        }

        public IEnumerable<Quest> GetQuests()
        {
            return quests.Values;
        }

        public void Dispose()
        {
            script.Dispose();
        }
    }
}