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