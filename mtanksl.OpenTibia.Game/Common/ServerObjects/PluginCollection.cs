using mtanksl.OpenTibia.Game.Plugins;
using NLua;
using OpenTibia.Game.Components;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class PluginCollection : IDisposable
    {
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

                        playerRotateItemPluginFactories.Add(openTibiaId, () => new LuaScriptingPlayerRotateItemPlugin("data/plugins/actions/" + filename) );
                    
                        break;

                    case "PlayerUseItem":

                        playerUseItemPluginFactories.Add(openTibiaId, () => new LuaScriptingPlayerUseItemPlugin("data/plugins/actions/" + filename) );
                  
                        break;

                    case "PlayerUseItemWithItem":
                        { 
                            bool allowFarUse = (bool)plugin["allowfaruse"];

                            if (allowFarUse)
                            {
                                playerUseItemWithItemPluginFactoriesAllowFarUse.Add(openTibiaId, () => new LuaScriptingPlayerUseItemWithtemPlugin("data/plugins/actions/" + filename) );
                            }
                            else
                            {
                                playerUseItemWithItemPluginFactories.Add(openTibiaId, () => new LuaScriptingPlayerUseItemWithtemPlugin("data/plugins/actions/" + filename) );
                            }
                        }
                        break;

                    case "PlayerUseItemWithCreature":
                        { 
                            bool allowFarUse = (bool)plugin["allowfaruse"];

                            if (allowFarUse)
                            {
                                playerUseItemWithCreaturePluginFactoriesAllowFarUse.Add(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin("data/plugins/actions/" + filename) );

                            }
                            else
                            {
                                playerUseItemWithCreaturePluginFactories.Add(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin("data/plugins/actions/" + filename) );
                            }
                        }
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

                        playerSayPluginFactories.Add(message, () => new LuaScriptingPlayerSayPlugin("data/plugins/talkactions/" + filename) );
                    
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

                        dialoguePluginFactories.Add(name, () => new LuaScriptingDialoguePlugin("data/plugins/npcs/" + filename) );
                                       
                        break;
                }
            }
        }

        private Dictionary<ushort, Func<PlayerRotateItemPlugin>> playerRotateItemPluginFactories = new Dictionary<ushort, Func<PlayerRotateItemPlugin>>();

        private Dictionary<ushort, PlayerRotateItemPlugin> playerRotateItemPlugins = new Dictionary<ushort, PlayerRotateItemPlugin>();

        public PlayerRotateItemPlugin GetPlayerRotateItemPlugin(ushort openTibiaId)
        {
            PlayerRotateItemPlugin plugin;

            if ( !playerRotateItemPlugins.TryGetValue(openTibiaId, out plugin) )
            {
                Func<PlayerRotateItemPlugin> factory;

                if (playerRotateItemPluginFactories.TryGetValue(openTibiaId, out factory) )
                {
                    plugin = factory();

                    plugin.Start();

                    playerRotateItemPlugins.Add(openTibiaId, plugin);
                }
            }

            return plugin;
        }

        private Dictionary<ushort, Func<PlayerUseItemPlugin>> playerUseItemPluginFactories = new Dictionary<ushort, Func<PlayerUseItemPlugin>>();

        private Dictionary<ushort, PlayerUseItemPlugin> playerUseItemPlugins = new Dictionary<ushort, PlayerUseItemPlugin>();

        public PlayerUseItemPlugin GetPlayerUseItemPlugin(ushort openTibiaId)
        {
            PlayerUseItemPlugin plugin;

            if ( !playerUseItemPlugins.TryGetValue(openTibiaId, out plugin) )
            {
                Func<PlayerUseItemPlugin> factory;

                if (playerUseItemPluginFactories.TryGetValue(openTibiaId, out factory) )
                {
                    plugin = factory();

                    plugin.Start();

                    playerUseItemPlugins.Add(openTibiaId, plugin);
                }
            }

            return plugin;
        }

        private Dictionary<ushort, Func<PlayerUseItemWithItemPlugin>> playerUseItemWithItemPluginFactoriesAllowFarUse = new Dictionary<ushort, Func<PlayerUseItemWithItemPlugin>>();

        private Dictionary<ushort, PlayerUseItemWithItemPlugin> playerUseItemWithCreaturePluginsAllowFarUse = new Dictionary<ushort, PlayerUseItemWithItemPlugin>();

        private Dictionary<ushort, Func<PlayerUseItemWithItemPlugin>> playerUseItemWithItemPluginFactories = new Dictionary<ushort, Func<PlayerUseItemWithItemPlugin>>();

        private Dictionary<ushort, PlayerUseItemWithItemPlugin> playerUseItemWithCreaturePlugins = new Dictionary<ushort, PlayerUseItemWithItemPlugin>();

        public PlayerUseItemWithItemPlugin GetPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId)
        {
            PlayerUseItemWithItemPlugin plugin;

            if (allowFarUse)
            {
                if ( !playerUseItemWithCreaturePluginsAllowFarUse.TryGetValue(openTibiaId, out plugin) )
                {
                    Func<PlayerUseItemWithItemPlugin> factory;

                    if ( playerUseItemWithItemPluginFactoriesAllowFarUse.TryGetValue(openTibiaId, out factory)  )
                    {
                        plugin = factory();

                        plugin.Start();

                        playerUseItemWithCreaturePluginsAllowFarUse.Add(openTibiaId, plugin);
                    }
                }
            }
            else
            {
                if ( !playerUseItemWithCreaturePlugins.TryGetValue(openTibiaId, out plugin) )
                {
                    Func<PlayerUseItemWithItemPlugin> factory;

                    if (playerUseItemWithItemPluginFactories.TryGetValue(openTibiaId, out factory) )
                    {
                        plugin = factory();

                        plugin.Start();

                        playerUseItemWithCreaturePlugins.Add(openTibiaId, plugin);
                    }
                }
            }

            return plugin;
        }

        private Dictionary<ushort, Func<PlayerUseItemWithCreaturePlugin>> playerUseItemWithCreaturePluginFactoriesAllowFarUse = new Dictionary<ushort, Func<PlayerUseItemWithCreaturePlugin>>();

        private Dictionary<ushort, PlayerUseItemWithCreaturePlugin> playerUseItemWithItemPluginsAllowFarUse = new Dictionary<ushort, PlayerUseItemWithCreaturePlugin>();

        private Dictionary<ushort, Func<PlayerUseItemWithCreaturePlugin>> playerUseItemWithCreaturePluginFactories = new Dictionary<ushort, Func<PlayerUseItemWithCreaturePlugin>>();

        private Dictionary<ushort, PlayerUseItemWithCreaturePlugin> playerUseItemWithItemPlugins = new Dictionary<ushort, PlayerUseItemWithCreaturePlugin>();

        public PlayerUseItemWithCreaturePlugin GetPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId)
        {
            PlayerUseItemWithCreaturePlugin plugin;

            if (allowFarUse)
            {
                if ( !playerUseItemWithItemPluginsAllowFarUse.TryGetValue(openTibiaId, out plugin) )
                {
                    Func<PlayerUseItemWithCreaturePlugin> factory;

                    if ( playerUseItemWithCreaturePluginFactoriesAllowFarUse.TryGetValue(openTibiaId, out factory) )
                    {
                        plugin = factory();

                        plugin.Start();

                        playerUseItemWithItemPluginsAllowFarUse.Add(openTibiaId, plugin);
                    }
                }
            }
            else
            {
                if ( !playerUseItemWithItemPlugins.TryGetValue(openTibiaId, out plugin) )
                {   
                    Func<PlayerUseItemWithCreaturePlugin> factory;

                    if ( playerUseItemWithCreaturePluginFactories.TryGetValue(openTibiaId, out factory) )
                    {
                        plugin = factory();

                        plugin.Start();

                        playerUseItemWithItemPlugins.Add(openTibiaId, plugin);
                    }
                }
            }

            return plugin;
        }

        private Dictionary<string, Func<PlayerSayPlugin>> playerSayPluginFactories = new Dictionary<string, Func<PlayerSayPlugin>>();

        private Dictionary<string, PlayerSayPlugin> playerSayPlugins = new Dictionary<string, PlayerSayPlugin>();

        public PlayerSayPlugin GetPlayerSayPlugin(string message)
        {
            PlayerSayPlugin plugin;

            if ( !playerSayPlugins.TryGetValue(message, out plugin) )
            {
                Func<PlayerSayPlugin> factory;

                if (playerSayPluginFactories.TryGetValue(message, out factory) )
                {
                    plugin = factory();

                    plugin.Start();

                    playerSayPlugins.Add(message, plugin);
                }
            }

            return plugin;
        }

        private Dictionary<string, Func<DialoguePlugin>> dialoguePluginFactories = new Dictionary<string, Func<DialoguePlugin>>();

        private List<DialoguePlugin> dialoguePlugins = new List<DialoguePlugin>();

        public DialoguePlugin GetDialoguePlugin(string name)
        {
            DialoguePlugin plugin = null;

            Func<DialoguePlugin> factory;

            if (dialoguePluginFactories.TryGetValue(name, out factory) )
            {
                plugin = factory();

                plugin.Start();

                dialoguePlugins.Add(plugin);
            }

            return plugin;
        }

        public void Stop()
        {
            foreach (var plugin in playerRotateItemPlugins.Values)
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemPlugins.Values)
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemWithCreaturePluginsAllowFarUse.Values)
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemWithCreaturePlugins.Values)
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemWithItemPluginsAllowFarUse.Values)
            {
                plugin.Stop();
            }

            foreach (var plugin in playerUseItemWithItemPlugins.Values)
            {
                plugin.Stop();
            }

            foreach (var plugin in playerSayPlugins.Values)
            {
                plugin.Stop();
            }

            foreach (var plugin in dialoguePlugins)
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