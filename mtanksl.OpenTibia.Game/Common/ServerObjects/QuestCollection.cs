using NLua;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class QuestCollection : IQuestCollection
    {
        private IServer server;

        public QuestCollection(IServer server)
        {
            this.server = server;
        }

        ~QuestCollection()
        {
            Dispose(false);
        }

        private ILuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/quests/config.lua"),
                server.PathResolver.GetFullPath("data/quests/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua") );

            foreach (LuaTable lQuest in ( (LuaTable)script["quests"] ).Values)
            {
                QuestConfig quest = new QuestConfig()
                {
                    Id = LuaScope.GetUInt16(lQuest["id"] ),

                    Name = LuaScope.GetString(lQuest["name"] )
                };

                foreach (LuaTable lMission in ( (LuaTable)lQuest["missions"] ).Values)
                {
                    MissionConfig mission = new MissionConfig()
                    {
                        Name = LuaScope.GetString(lMission["name"] ),

                        Description = LuaScope.GetString(lMission["description"] ),

                        StorageKey = LuaScope.GetInt32(lMission["storagekey"] ),

                        StorageValue = LuaScope.GetInt32(lMission["storagevalue"] )
                    };

                    quest.Missions.Add(mission);
                }

                quests.Add(quest.Id, quest);
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>
       
        public object GetValue(string key)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(QuestCollection) );
            }

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

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    if (script != null)
                    {
                        script.Dispose();
                    }
                }
            }
        }
    }
}