using mtanksl.OpenTibia.Game.Plugins;
using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class PluginCollection : IDisposable
    {
        private LuaScope script;

        public PluginCollection(Server server)
        {
            script = server.LuaScripts.Create(server.PathResolver.GetFullPath("data/plugins/config.lua") );

            foreach (LuaTable plugin in ( (LuaTable)script["plugins"] ).Values)
            {
                string type = (string)plugin["type"];

                switch (type)
                {
                    case "PlayerRotateItem":
                    { 
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        string filename = (string)plugin["filename"];

                        playerRotateItemPlugins.Add(openTibiaId, new LuaScriptingPlayerRotateItemPlugin("data/plugins/" + filename) );
                    }
                    break;

                    case "PlayerUseItem":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        string filename = (string)plugin["filename"];

                        playerUseItemPlugins.Add(openTibiaId, new LuaScriptingPlayerUseItemPlugin("data/plugins/" + filename) );
                    }
                    break;

                    case "PlayerSay":
                    {
                        string message = (string)plugin["message"];

                        string filename = (string)plugin["filename"];

                        playerSayPlugins.Add(message, new LuaScriptingPlayerSayPlugin("data/plugins/" + filename) );
                    }
                    break;

                    case "Conversation":
                    {
                        string name = (string)plugin["name"];

                        string filename = (string)plugin["filename"];

                        conversationPlugin.Add(name, npc => new LuaScriptingConversationPlugin(npc, "data/plugins/" + filename) );
                    }
                    break;
                }
            }
        }

        private Dictionary<ushort, PlayerRotateItemPlugin> playerRotateItemPlugins = new Dictionary<ushort, PlayerRotateItemPlugin>();

        public Dictionary<ushort, PlayerRotateItemPlugin> PlayerRotateItemPlugins
        {
            get
            {
                return playerRotateItemPlugins;
            }
        }

        private Dictionary<ushort, PlayerUseItemPlugin> playerUseItemPlugins = new Dictionary<ushort, PlayerUseItemPlugin>();

        public Dictionary<ushort, PlayerUseItemPlugin> PlayerUseItemPlugins
        {
            get
            {
                return playerUseItemPlugins;
            }
        }

        private Dictionary<string, PlayerSayPlugin> playerSayPlugins = new Dictionary<string, PlayerSayPlugin>();

        public Dictionary<string, PlayerSayPlugin> PlayerSayPlugins
        {
            get
            {
                return playerSayPlugins;
            }
        }

        private Dictionary<string, Func<Npc, ConversationPlugin>> conversationPlugin = new Dictionary<string, Func<Npc, ConversationPlugin>>();

        public Dictionary<string, Func<Npc, ConversationPlugin>> ConversationPlugins
        {
            get
            {
                return conversationPlugin;
            }
        }

        public void Dispose()
        {
            script.Dispose();
        }
    }
}