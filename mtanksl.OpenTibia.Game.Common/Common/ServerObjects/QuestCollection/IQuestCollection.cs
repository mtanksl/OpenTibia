using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IQuestCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        QuestConfig GetQuestById(ushort id);

        IEnumerable<QuestConfig> GetQuests();
    }
}