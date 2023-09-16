using mtanksl.OpenTibia.Game.Plugins;
using NLua;
using OpenTibia.Game.Components;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class PluginCollection : IDisposable
    {
        private class PluginDictionaryCached<TKey, TValue> where TValue : Plugin
        {
            private Dictionary<TKey, Func<TValue>> factories = new Dictionary<TKey, Func<TValue>>();

            private Dictionary<TKey, TValue> plugins = new Dictionary<TKey, TValue>();

            public void AddPlugin(TKey key, Func<TValue> factory)
            {
                factories.Add(key, factory);
            }

            public TValue GetPlugin(TKey key)
            {
                TValue plugin;

                if ( !plugins.TryGetValue(key, out plugin) )
                {
                    Func<TValue> factory;

                    if (factories.TryGetValue(key, out factory) )
                    {
                        plugin = factory();

                        plugin.Start();

                        plugins.Add(key, plugin);
                    }
                }

                return plugin;
            }

            public IEnumerable<TValue> GetPlugins()
            {
                return plugins.Values;
            }
        }

        private class PluginDictionary<TKey, TValue> where TValue : Plugin
        {
            private Dictionary<TKey, Func<TValue>> factories = new Dictionary<TKey, Func<TValue>>();

            private List<TValue> plugins = new List<TValue>();

            public void AddPlugin(TKey key, Func<TValue> factory)
            {
                factories.Add(key, factory);
            }

            public TValue GetPlugin(TKey key)
            {
                TValue plugin = null;

                Func<TValue> factory;

                if (factories.TryGetValue(key, out factory) )
                {
                    plugin = factory();

                    plugin.Start();

                    plugins.Add(plugin);
                }

                return plugin;
            }

            public IEnumerable<TValue> GetPlugins()
            {
                return plugins;
            }
        }

        private Server server;

        public PluginCollection(Server server)
        {
            this.server = server;
        }

        private LuaScope script;

        public void Start()
        {
            script = server.LuaScripts.Create(server.PathResolver.GetFullPath("data/plugins/config.lua") );

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.actions"] ).Values)
            {
                string type = (string)plugin["type"];

                ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                string filename = (string)plugin["filename"];

                switch (type)
                {
                    case "PlayerRotateItem":

                        playerRotateItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerRotateItemPlugin("data/plugins/actions/" + filename) );
                    
                        break;

                    case "PlayerUseItem":

                        playerUseItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemPlugin("data/plugins/actions/" + filename) );
                  
                        break;

                    case "PlayerUseItemWithItem":
                        { 
                            bool allowFarUse = (bool)plugin["allowfaruse"];

                            if (allowFarUse)
                            {
                                playerUseItemWithItemPluginsAllowFarUse.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithtemPlugin("data/plugins/actions/" + filename) );
                            }
                            else
                            {
                                playerUseItemWithItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithtemPlugin("data/plugins/actions/" + filename) );
                            }
                        }
                        break;

                    case "PlayerUseItemWithCreature":
                        { 
                            bool allowFarUse = (bool)plugin["allowfaruse"];

                            if (allowFarUse)
                            {
                                playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin("data/plugins/actions/" + filename) );
                            }
                            else
                            {
                                playerUseItemWithCreaturePlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin("data/plugins/actions/" + filename) );
                            }
                        }
                        break;

                    case "PlayerMoveItem":
                        
                        playerMoveItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerMoveItemPlugin("data/plugins/actions/" + filename) );

                        break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.talkactions"] ).Values)
            {
                string type = (string)plugin["type"];

                string message = (string)plugin["message"];

                string filename = (string)plugin["filename"];

                switch (type)
                {
                    case "PlayerSay":

                        playerSayPlugins.AddPlugin(message, () => new LuaScriptingPlayerSayPlugin("data/plugins/talkactions/" + filename) );
                    
                        break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.npcs"] ).Values)
            {
                string type = (string)plugin["type"];

                string name = (string)plugin["name"];

                string filename = (string)plugin["filename"];

                switch (type)
                {
                    case "Dialogue":

                        dialoguePlugins.AddPlugin(name, () => new LuaScriptingDialoguePlugin("data/plugins/npcs/" + filename) );
                                       
                        break;
                }
            }
        }

        private PluginDictionaryCached<ushort, PlayerRotateItemPlugin> playerRotateItemPlugins = new PluginDictionaryCached<ushort, PlayerRotateItemPlugin>();

        public PlayerRotateItemPlugin GetPlayerRotateItemPlugin(ushort openTibiaId)
        {
            return playerRotateItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, PlayerUseItemPlugin> playerUseItemPlugins = new PluginDictionaryCached<ushort, PlayerUseItemPlugin>();

        public PlayerUseItemPlugin GetPlayerUseItemPlugin(ushort openTibiaId)
        {
            return playerUseItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin> playerUseItemWithItemPluginsAllowFarUse = new PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin>();

        private PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin> playerUseItemWithItemPlugins = new PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin>();

        public PlayerUseItemWithItemPlugin GetPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId)
        {
            if (allowFarUse)
            {
                return playerUseItemWithItemPluginsAllowFarUse.GetPlugin(openTibiaId);
            }
            else
            {
                return playerUseItemWithItemPlugins.GetPlugin(openTibiaId);
            }
        }

        private PluginDictionaryCached<ushort, PlayerUseItemWithCreaturePlugin> playerUseItemWithCreaturePluginsAllowFarUse = new PluginDictionaryCached<ushort, PlayerUseItemWithCreaturePlugin>();

        private PluginDictionaryCached<ushort, PlayerUseItemWithCreaturePlugin> playerUseItemWithCreaturePlugins = new PluginDictionaryCached<ushort, PlayerUseItemWithCreaturePlugin>();

        public PlayerUseItemWithCreaturePlugin GetPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId)
        {
            if (allowFarUse)
            {
                return playerUseItemWithCreaturePluginsAllowFarUse.GetPlugin(openTibiaId);
            }
            else
            {
                return playerUseItemWithCreaturePlugins.GetPlugin(openTibiaId);
            }
        }

        private PluginDictionaryCached<ushort, PlayerMoveItemPlugin> playerMoveItemPlugins = new PluginDictionaryCached<ushort, PlayerMoveItemPlugin>();

        public PlayerMoveItemPlugin GetPlayerMoveItemPlugin(ushort openTibiaId)
        {
            return playerMoveItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<string, PlayerSayPlugin> playerSayPlugins = new PluginDictionaryCached<string, PlayerSayPlugin>();

        public PlayerSayPlugin GetPlayerSayPlugin(string message)
        {
            return playerSayPlugins.GetPlugin(message);
        }

        private PluginDictionary<string, DialoguePlugin> dialoguePlugins = new PluginDictionary<string, DialoguePlugin>();

        public DialoguePlugin GetDialoguePlugin(string name)
        {
            return dialoguePlugins.GetPlugin(name);
        }

        public void Stop()
        {
            foreach (var plugin in playerRotateItemPlugins.GetPlugins() )
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemPlugins.GetPlugins() )
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemWithCreaturePluginsAllowFarUse.GetPlugins() )
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemWithCreaturePlugins.GetPlugins() )
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemWithItemPluginsAllowFarUse.GetPlugins() )
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemWithItemPlugins.GetPlugins() )
            {
                plugin.Stop();
            }

            foreach (var plugin in playerMoveItemPlugins.GetPlugins() )
            {
                plugin.Stop();
            }

            foreach (var plugin in playerSayPlugins.GetPlugins() )
            {
                plugin.Stop();
            }

            foreach (var plugin in dialoguePlugins.GetPlugins() )
            {
                plugin.Stop();
            }
        }

        public void Dispose()
        {
            script.Dispose();
        }
    }
}