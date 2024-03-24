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

        private LuaScope script;

        public void Start()
        {
            script = server.LuaScripts.Create(server.PathResolver.GetFullPath("data/lib.lua"), server.PathResolver.GetFullPath("data/quests/lib.lua"), server.PathResolver.GetFullPath("data/quests/config.lua") );

            foreach (LuaTable lQuest in ( (LuaTable)script["quests"] ).Values)
            {
                QuestConfig quest = new QuestConfig()
                {
                    Id = (ushort)(long)lQuest["id"],

                    Name = (string)lQuest["name"]
                };

                foreach (LuaTable lMission in ( (LuaTable)lQuest["missions"] ).Values)
                {
                    MissionConfig mission = new MissionConfig()
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

        public object GetValue(string key)
        {
            return script[key];
        }

        private Dictionary<ushort, QuestConfig> quests = new Dictionary<ushort, QuestConfig>();

        public QuestConfig GetQuestById(ushort id)
        {
            QuestConfig quest;

            if (quests.TryGetValue(id, out quest) )
            {
                return quest;
            }

            return null;
        }

        public IEnumerable<QuestConfig> GetQuests()
        {
            return quests.Values;
        }

        public void Dispose()
        {
            script.Dispose();
        }
    }
}