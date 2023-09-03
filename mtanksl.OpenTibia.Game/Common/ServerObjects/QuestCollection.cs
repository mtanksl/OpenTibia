using NLua;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class QuestCollection : IDisposable
    {
        LuaScope script;

        public QuestCollection(Server server)
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

                quests.Add(quest);
            }
        }

        private List<Quest> quests = new List<Quest>();

        public Quest GetQuestById(ushort id)
        {
            return quests.Where(q => q.Id == id).FirstOrDefault();
        }

        public IEnumerable<Quest> GetQuests()
        {
            return quests;
        }

        public void Dispose()
        {
            script.Dispose();
        }
    }
}