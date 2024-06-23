using OpenTibia.Common.Objects;
using OpenTibia.Game.GameObjectScripts;
using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGameObjectScriptCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        GameObjectScript<Item> GetItemGameObjectScript(ushort openTibiaId);

        GameObjectScript<Player> GetPlayerGameObjectScript(string name);

        GameObjectScript<Monster> GetMonsterGameObjectScript(string name);

        GameObjectScript<Npc> GetNpcGameObjectScript(string name);
    }
}