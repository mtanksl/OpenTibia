using NLua;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class PluginCollection : IPluginCollection
    {
        private class PluginListCached<TValue> where TValue : Plugin
        {
            private List<TValue> plugins = new List<TValue>();

            public void AddPlugin(TValue plugin)
            {
                plugin.Start();

                plugins.Add(plugin);
            }

            public IEnumerable<TValue> GetPlugins()
            {
                return plugins;
            }
        }

        private class PluginDictionaryCached<TKey, TValue> where TValue : Plugin
        {
            private Dictionary<TKey, TValue> plugins = new Dictionary<TKey, TValue>();

            public void AddPlugin(TKey key, TValue plugin)
            {
                plugin.Start();

                plugins.Add(key, plugin);
            }

            public TValue GetPlugin(TKey key)
            {
                TValue plugin;

                plugins.TryGetValue(key, out plugin);

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
                Func<TValue> factory;

                if (factories.TryGetValue(key, out factory) )
                {
                    TValue plugin = factory();

                    plugin.Start();

                    plugins.Add(plugin);

                    return plugin;
                }

                return null;
            }

            public IEnumerable<TValue> GetPlugins()
            {
                return plugins;
            }
        }

        private class AutoLoadPlugin : IDisposable
        {
            private ILuaScope script;

            ~AutoLoadPlugin()
            {
                Dispose(false);
            }

            public AutoLoadPlugin(PluginCollection pluginCollection, string filePath)
            {
                ILuaScope scripts = pluginCollection.server.LuaScripts.LoadLib(
                    pluginCollection.server.PathResolver.GetFullPath("data/plugins/scripts/lib.lua"), 
                    pluginCollection.server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    pluginCollection.server.PathResolver.GetFullPath("data/lib.lua") );
                
                var initializations = new List<(string Type, LuaTable Parameters)>();

                scripts["registerplugin"] = (string type, LuaTable parameters) =>
                {
                    initializations.Add( (type, parameters) );
                };

                script = pluginCollection.server.LuaScripts.LoadScript(pluginCollection.server.PathResolver.GetFullPath(filePath), scripts);

                scripts["registerplugin"] = null;

                foreach (var initialization in initializations)
                {
                    if (initialization.Type == "actions")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "PlayerRotateItem":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                pluginCollection.AddPlayerRotateItemPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "PlayerUseItem":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                pluginCollection.AddPlayerUseItemPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "PlayerUseItemWithItem":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                bool allowFarUse = LuaScope.GetBoolean(initialization.Parameters["allowfaruse"] );

                                pluginCollection.AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, script, initialization.Parameters);                     
                            }
                            break;

                            case "PlayerUseItemWithCreature":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                bool allowFarUse = LuaScope.GetBoolean(initialization.Parameters["allowfaruse"] );

                                pluginCollection.AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "PlayerMoveItem":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                pluginCollection.AddPlayerMoveItemPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "PlayerMoveCreature":
                            {
                                string name = LuaScope.GetString(initialization.Parameters["name"] );

                                pluginCollection.AddPlayerMoveCreaturePlugin(name, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "movements")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "CreatureStepIn":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                pluginCollection.AddCreatureStepInPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "CreatureStepOut":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                pluginCollection.AddCreatureStepOutPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "InventoryEquip":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                pluginCollection.AddInventoryEquipPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "InventoryDeEquip":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                pluginCollection.AddInventoryDeEquipPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "talkactions")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "PlayerSay":
                            {
                                string message = LuaScope.GetString(initialization.Parameters["message"] );

                                pluginCollection.AddPlayerSayPlugin(message, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "creaturescripts")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "PlayerLogin":
                            {
                                pluginCollection.AddPlayerLoginPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "PlayerLogout":
                            {
                                pluginCollection.AddPlayerLogoutPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "PlayerAdvanceLevel":
                            {
                                pluginCollection.AddPlayerAdvanceLevelPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "PlayerAdvanceSkill":
                            {
                                pluginCollection.AddPlayerAdvanceSkillPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "CreatureDeath":
                            {
                                pluginCollection.AddCreatureDeathPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "PlayerEarnAchievement":
                            {
                                pluginCollection.AddPlayerEarnAchievementPlugin(script, initialization.Parameters);
                            }
                            break;
                        }
                    }                    
                    else if (initialization.Type == "globalevents")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "ServerStartup":
                            {
                                pluginCollection.AddServerStartupPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "ServerShutdown":
                            {
                                pluginCollection.AddServerShutdownPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "ServerSave":
                            {
                                pluginCollection.AddServerSavePlugin(script, initialization.Parameters);
                            }
                            break;

                            case "ServerRecord":
                            {
                                pluginCollection.AddServerRecordPlugin(script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "npcs")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "Dialogue":
                            {
                                string name = LuaScope.GetString(initialization.Parameters["name"] );

                                pluginCollection.AddDialoguePlugin(name, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "spells")
                    {
                        bool requiresTarget = LuaScope.GetBoolean(initialization.Parameters["requirestarget"] );

                        Spell spell = new Spell()
                        {
                            Words = LuaScope.GetString(initialization.Parameters["words"] ),

                            Name = LuaScope.GetString(initialization.Parameters["name"] ),

                            Group = LuaScope.GetString(initialization.Parameters["group"] ),

                            Cooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(initialization.Parameters["cooldown"] ) ),

                            GroupCooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(initialization.Parameters["groupcooldown"] ) ),

                            Level = LuaScope.GetInt32(initialization.Parameters["level"] ),

                            Mana = LuaScope.GetInt32(initialization.Parameters["mana"] ),

                            Soul = LuaScope.GetInt32(initialization.Parameters["soul"] ),

                            ConjureOpenTibiaId = LuaScope.GetNullableUInt16(initialization.Parameters["conjureopentibiaid"] ),

                            ConjureCount = LuaScope.GetNullableInt32(initialization.Parameters["conjurecount"] ),

                            Premium = LuaScope.GetBoolean(initialization.Parameters["premium"] ),

                            Vocations = ( (LuaTable)initialization.Parameters["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                        };

                        pluginCollection.AddSpellPlugin(requiresTarget, script, initialization.Parameters, spell);
                    }
                    else if (initialization.Type == "runes")
                    {
                        bool requiresTarget = LuaScope.GetBoolean(initialization.Parameters["requirestarget"] );

                        Rune rune = new Rune()
                        {
                            OpenTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] ),

                            Name = LuaScope.GetString(initialization.Parameters["name"] ),

                            Group = LuaScope.GetString(initialization.Parameters["group"] ),

                            GroupCooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(initialization.Parameters["groupcooldown"] ) ),

                            Level = LuaScope.GetInt32(initialization.Parameters["level"] ),

                            MagicLevel = LuaScope.GetInt32(initialization.Parameters["magiclevel"] ),

                            Vocations = ( (LuaTable)initialization.Parameters["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                        };

                        pluginCollection.AddRunePlugin(requiresTarget, script, initialization.Parameters, rune);
                    }
                    else if (initialization.Type == "weapons")
                    {
                        Weapon weapon = new Weapon()
                        {
                            OpenTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] ),

                            Level = LuaScope.GetInt32(initialization.Parameters["level"] ),

                            Mana = LuaScope.GetInt32(initialization.Parameters["mana"] ),

                            Vocations = ( (LuaTable)initialization.Parameters["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                        };

                        pluginCollection.AddWeaponPlugin( script, initialization.Parameters, weapon);
                    }
                    else if (initialization.Type == "ammunitions")
                    {
                        Ammunition ammunition = new Ammunition()
                        {
                            OpenTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] ),

                            Level = LuaScope.GetInt32(initialization.Parameters["level"] )
                        };

                        pluginCollection.AddAmmunitionPlugin(script, initialization.Parameters, ammunition);
                    }
                    else if (initialization.Type == "raids")
                    {
                        Raid raid = new Raid()
                        {
                            Name = LuaScope.GetString(initialization.Parameters["name"] ),

                            Repeatable = LuaScope.GetBoolean(initialization.Parameters["repeatable"] ),

                            Interval = LuaScope.GetInt32(initialization.Parameters["interval"] ),

                            Chance = LuaScope.GetDouble(initialization.Parameters["chance"] ),

                            Enabled = LuaScope.GetBoolean(initialization.Parameters["enabled"] )
                        };

                        pluginCollection.AddRaidPlugin(script, initialization.Parameters, raid);
                    }
                }
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

        private IServer server;

        public PluginCollection(IServer server)
        {
            this.server = server;
        }

        ~PluginCollection()
        {
            Dispose(false);
        }

        private ILuaScope script;

        private List<AutoLoadPlugin> autoLoadPlugins = new List<AutoLoadPlugin>();

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/plugins/config.lua"), 
                server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                server.PathResolver.GetFullPath("data/lib.lua") );

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.actions"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "PlayerRotateItem":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        AddPlayerRotateItemPlugin(openTibiaId, fileName, plugin);
                    }
                    break;

                    case "PlayerUseItem":
                    { 
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        AddPlayerUseItemPlugin(openTibiaId, fileName, plugin);
                    }
                    break;

                    case "PlayerUseItemWithItem":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        bool allowFarUse = LuaScope.GetBoolean(plugin["allowfaruse"] );

                        AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, fileName, plugin);
                    }
                    break;

                    case "PlayerUseItemWithCreature":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        bool allowFarUse = LuaScope.GetBoolean(plugin["allowfaruse"] );

                        AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, fileName, plugin);
                    }
                    break;

                    case "PlayerMoveItem":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        AddPlayerMoveItemPlugin(openTibiaId, fileName, plugin);
                    }
                    break;

                    case "PlayerMoveCreature":
                    {
                        string name = LuaScope.GetString(plugin["name"] );

                        AddPlayerMoveCreaturePlugin(name, fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.movements"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "CreatureStepIn":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        AddCreatureStepInPlugin(openTibiaId, fileName, plugin);
                    }
                    break;

                    case "CreatureStepOut":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        AddCreatureStepOutPlugin(openTibiaId, fileName, plugin);
                    }
                    break;

                    case "InventoryEquip":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        AddInventoryEquipPlugin(openTibiaId, fileName, plugin);
                    }
                    break;

                    case "InventoryDeEquip":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        AddInventoryDeEquipPlugin(openTibiaId, fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.talkactions"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "PlayerSay":
                    {
                        string message = LuaScope.GetString(plugin["message"] );

                        AddPlayerSayPlugin(message, fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.creaturescripts"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "PlayerLogin":
                    {
                        AddPlayerLoginPlugin(fileName, plugin);
                    }
                    break;

                    case "PlayerLogout":
                    {
                        AddPlayerLogoutPlugin(fileName, plugin);
                    }
                    break;

                    case "PlayerAdvanceLevel":
                    {
                        AddPlayerAdvanceLevelPlugin(fileName, plugin);
                    }
                    break;

                    case "PlayerAdvanceSkill":
                    {
                        AddPlayerAdvanceSkillPlugin(fileName, plugin);
                    }
                    break;

                    case "CreatureDeath":
                    {
                        AddCreatureDeathPlugin(fileName, plugin);
                    }
                    break;

                    case "PlayerEarnAchievement":
                    {
                        AddPlayerEarnAchievementPlugin(fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.globalevents"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "ServerStartup":
                    {
                        AddServerStartupPlugin(fileName, plugin);
                    }
                    break;

                    case "ServerShutdown":
                    {
                        AddServerShutdownPlugin(fileName, plugin);
                    }
                    break;

                    case "ServerSave":
                    {
                        AddServerSavePlugin(fileName, plugin);
                    }
                    break;
                        
                    case "ServerRecord":
                    {
                        AddServerRecordPlugin(fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.npcs"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "Dialogue":
                    {
                        string name = LuaScope.GetString(plugin["name"] );

                        AddDialoguePlugin(name, fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.spells"] ).Values)
            {
                string fileName = LuaScope.GetString(plugin["filename"] );

                bool requiresTarget = LuaScope.GetBoolean(plugin["requirestarget"] );

                Spell spell = new Spell()
                {
                    Words = LuaScope.GetString(plugin["words"] ),

                    Name = LuaScope.GetString(plugin["name"] ),

                    Group = LuaScope.GetString(plugin["group"] ),

                    Cooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(plugin["cooldown"] ) ),

                    GroupCooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(plugin["groupcooldown"] ) ),

                    Level = LuaScope.GetInt32(plugin["level"] ),

                    Mana = LuaScope.GetInt32(plugin["mana"] ),

                    Soul = LuaScope.GetInt32(plugin["soul"] ),

                    ConjureOpenTibiaId = LuaScope.GetNullableUInt16(plugin["conjureopentibiaid"] ),

                    ConjureCount = LuaScope.GetNullableInt32(plugin["conjurecount"] ),

                    Premium = LuaScope.GetBoolean(plugin["premium"] ),

                    Vocations = ( (LuaTable)plugin["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                };

                AddSpellPlugin(requiresTarget, fileName, plugin, spell);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.runes"] ).Values)
            {
                string fileName = LuaScope.GetString(plugin["filename"] );

                bool requiresTarget = LuaScope.GetBoolean(plugin["requirestarget"] );

                Rune rune = new Rune()
                {
                    OpenTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] ),

                    Name = LuaScope.GetString(plugin["name"] ),

                    Group = LuaScope.GetString(plugin["group"] ),

                    GroupCooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(plugin["groupcooldown"] ) ),

                    Level = LuaScope.GetInt32(plugin["level"] ),

                    MagicLevel = LuaScope.GetInt32(plugin["magiclevel"] ),
                          
                    Vocations = ( (LuaTable)plugin["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                };

                AddRunePlugin(requiresTarget, fileName, plugin, rune);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.weapons"] ).Values)
            {
                string fileName = LuaScope.GetString(plugin["filename"] );

                Weapon weapon = new Weapon()
                {
                    OpenTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] ),

                    Level = LuaScope.GetInt32(plugin["level"] ),

                    Mana = LuaScope.GetInt32(plugin["mana"] ),

                    Vocations = ( (LuaTable)plugin["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                };

                AddWeaponPlugin(fileName, plugin, weapon);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.ammunitions"] ).Values)
            {
                string fileName = LuaScope.GetString(plugin["filename"] );

                Ammunition ammunition = new Ammunition()
                {
                    OpenTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] ),

                    Level = LuaScope.GetInt32(plugin["level"])
                };

                AddAmmunitionPlugin(fileName, plugin, ammunition);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.raids"] ).Values)
            {
                string fileName = LuaScope.GetString(plugin["filename"] );

                Raid raid = new Raid()
                {
                    Name = LuaScope.GetString(plugin["name"] ),

                    Repeatable = LuaScope.GetBoolean(plugin["repeatable"] ),

                    Interval = LuaScope.GetInt32(plugin["interval"] ),

                    Chance = LuaScope.GetDouble(plugin["chance"] ),

                    Enabled = LuaScope.GetBoolean(plugin["enabled"] )
                };

                AddRaidPlugin(fileName, plugin, raid);
            }

            foreach (var filePath in Directory.GetFiles(server.PathResolver.GetFullPath("data/plugins/scripts"), "*.lua", SearchOption.AllDirectories) )
            {
                string fileName = Path.GetFileName(filePath);

                if (fileName != "lib.lua")
                {
                    autoLoadPlugins.Add(new AutoLoadPlugin(this, filePath) );
                }
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>
      
        public object GetValue(string key)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(PluginCollection) );
            }

            return script[key];
        }

        private PluginDictionaryCached<ushort, PlayerRotateItemPlugin> playerRotateItemPlugins = new PluginDictionaryCached<ushort, PlayerRotateItemPlugin>();

        private void AddPlayerRotateItemPlugin(ushort openTibiaId, PlayerRotateItemPlugin playerRotateItemPlugin)
        {
            playerRotateItemPlugins.AddPlugin(openTibiaId, playerRotateItemPlugin);
        }

        public void AddPlayerRotateItemPlugin(ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerRotateItemPlugin(openTibiaId, new LuaScriptingPlayerRotateItemPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerRotateItemPlugin(openTibiaId, (PlayerRotateItemPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerRotateItemPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddPlayerRotateItemPlugin(openTibiaId, new LuaScriptingPlayerRotateItemPlugin(script, parameters) );
        }

        public PlayerRotateItemPlugin GetPlayerRotateItemPlugin(ushort openTibiaId)
        {
            return playerRotateItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, PlayerUseItemPlugin> playerUseItemPlugins = new PluginDictionaryCached<ushort, PlayerUseItemPlugin>();

        private void AddPlayerUseItemPlugin(ushort openTibiaId, PlayerUseItemPlugin playerUseItemPlugin)
        {
            playerUseItemPlugins.AddPlugin(openTibiaId, playerUseItemPlugin);
        }

        public void AddPlayerUseItemPlugin(ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerUseItemPlugin(openTibiaId, new LuaScriptingPlayerUseItemPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerUseItemPlugin(openTibiaId, (PlayerUseItemPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerUseItemPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddPlayerUseItemPlugin(openTibiaId, new LuaScriptingPlayerUseItemPlugin(script, parameters) );
        }

        public PlayerUseItemPlugin GetPlayerUseItemPlugin(ushort openTibiaId)
        {
            return playerUseItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin> playerUseItemWithItemPluginsAllowFarUse = new PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin>();
        private PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin> playerUseItemWithItemPlugins = new PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin>();

        private void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, PlayerUseItemWithItemPlugin playerUseItemWithItemPlugin)
        {
            if (allowFarUse)
            {
                playerUseItemWithItemPluginsAllowFarUse.AddPlugin(openTibiaId, playerUseItemWithItemPlugin);
            }
            else
            {
                playerUseItemWithItemPlugins.AddPlugin(openTibiaId, playerUseItemWithItemPlugin);
            }
        }
           
        public void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, new LuaScriptingPlayerUseItemWithItemPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, (PlayerUseItemWithItemPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, new LuaScriptingPlayerUseItemWithItemPlugin(script, parameters) );
        }

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

        private void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, PlayerUseItemWithCreaturePlugin playerUseItemWithCreaturePlugin)
        {
            if (allowFarUse)
            {
                playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(openTibiaId, playerUseItemWithCreaturePlugin);
            }
            else
            {
                playerUseItemWithCreaturePlugins.AddPlugin(openTibiaId, playerUseItemWithCreaturePlugin);
            }
        }
        
        public void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, new LuaScriptingPlayerUseItemWithCreaturePlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, (PlayerUseItemWithCreaturePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, new LuaScriptingPlayerUseItemWithCreaturePlugin(script, parameters) );
        }

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

        private PluginDictionaryCached<string, PlayerMoveCreaturePlugin> playerMoveCreaturePlugins = new PluginDictionaryCached<string, PlayerMoveCreaturePlugin>();

        private void AddPlayerMoveCreaturePlugin(string name, PlayerMoveCreaturePlugin playerMoveCreaturePlugin)
        {
            playerMoveCreaturePlugins.AddPlugin(name, playerMoveCreaturePlugin);
        }

        public void AddPlayerMoveCreaturePlugin(string name, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerMoveCreaturePlugin(name, new LuaScriptingPlayerMoveCreaturePlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerMoveCreaturePlugin(name, (PlayerMoveCreaturePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerMoveCreaturePlugin(string name, ILuaScope script, LuaTable parameters)
        {
            AddPlayerMoveCreaturePlugin(name, new LuaScriptingPlayerMoveCreaturePlugin(script, parameters) );
        }

        public PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(string name)
        {
            return playerMoveCreaturePlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<ushort, PlayerMoveItemPlugin> playerMoveItemPlugins = new PluginDictionaryCached<ushort, PlayerMoveItemPlugin>();

        private void AddPlayerMoveItemPlugin(ushort openTibiaId, PlayerMoveItemPlugin playerMoveItemPlugin)
        {
            playerMoveItemPlugins.AddPlugin(openTibiaId, playerMoveItemPlugin);
        }

        public void AddPlayerMoveItemPlugin(ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerMoveItemPlugin(openTibiaId, new LuaScriptingPlayerMoveItemPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerMoveItemPlugin(openTibiaId, (PlayerMoveItemPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerMoveItemPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddPlayerMoveItemPlugin(openTibiaId, new LuaScriptingPlayerMoveItemPlugin(script, parameters) );
        }

        public PlayerMoveItemPlugin GetPlayerMoveItemPlugin(ushort openTibiaId)
        {
            return playerMoveItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, CreatureStepInPlugin> creatureStepInPlugins = new PluginDictionaryCached<ushort, CreatureStepInPlugin>();

        private void AddCreatureStepInPlugin(ushort openTibiaId, CreatureStepInPlugin creatureStepInPlugin)
        {
            creatureStepInPlugins.AddPlugin(openTibiaId, creatureStepInPlugin);
        }
                    
        public void AddCreatureStepInPlugin(ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddCreatureStepInPlugin(openTibiaId, new LuaScriptingCreatureStepInPlugin(fileName, parameters) );
            }
            else
            {
                AddCreatureStepInPlugin(openTibiaId, (CreatureStepInPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddCreatureStepInPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddCreatureStepInPlugin(openTibiaId, new LuaScriptingCreatureStepInPlugin(script, parameters) );
        }

        public CreatureStepInPlugin GetCreatureStepInPlugin(ushort openTibiaId)
        {
            return creatureStepInPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, CreatureStepOutPlugin> creatureStepOutPlugins = new PluginDictionaryCached<ushort, CreatureStepOutPlugin>();

        private void AddCreatureStepOutPlugin(ushort openTibiaId, CreatureStepOutPlugin creatureStepOutPlugin)
        {
            creatureStepOutPlugins.AddPlugin(openTibiaId, creatureStepOutPlugin);
        }

        public void AddCreatureStepOutPlugin(ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddCreatureStepOutPlugin(openTibiaId, new LuaScriptingCreatureStepOutPlugin(fileName, parameters) );
            }
            else
            {
                AddCreatureStepOutPlugin(openTibiaId, (CreatureStepOutPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddCreatureStepOutPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddCreatureStepOutPlugin(openTibiaId, new LuaScriptingCreatureStepOutPlugin(script, parameters) );
        }

        public CreatureStepOutPlugin GetCreatureStepOutPlugin(ushort openTibiaId)
        {
            return creatureStepOutPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, InventoryEquipPlugin> inventoryEquipPlugins = new PluginDictionaryCached<ushort, InventoryEquipPlugin>();

        private void AddInventoryEquipPlugin(ushort openTibiaId, InventoryEquipPlugin inventoryEquipPlugin)
        {
            inventoryEquipPlugins.AddPlugin(openTibiaId, inventoryEquipPlugin);
        }

        public void AddInventoryEquipPlugin(ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddInventoryEquipPlugin(openTibiaId, new LuaScriptingInventoryEquipPlugin(fileName, parameters) );
            }
            else
            {
                AddInventoryEquipPlugin(openTibiaId, (InventoryEquipPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddInventoryEquipPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddInventoryEquipPlugin(openTibiaId, new LuaScriptingInventoryEquipPlugin(script, parameters) );
        }

        public InventoryEquipPlugin GetInventoryEquipPlugin(ushort openTibiaId)
        {
            return inventoryEquipPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, InventoryDeEquipPlugin> inventoryDeEquipPlugins = new PluginDictionaryCached<ushort, InventoryDeEquipPlugin>();

        private void AddInventoryDeEquipPlugin(ushort openTibiaId, InventoryDeEquipPlugin inventoryDeEquipPlugin)
        {
            inventoryDeEquipPlugins.AddPlugin(openTibiaId, inventoryDeEquipPlugin);
        }

        public void AddInventoryDeEquipPlugin(ushort openTibiaId, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddInventoryDeEquipPlugin(openTibiaId, new LuaScriptingInventoryDeEquipPlugin(fileName, parameters) );
            }
            else
            {
                AddInventoryDeEquipPlugin(openTibiaId, (InventoryDeEquipPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddInventoryDeEquipPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters)
        {
            AddInventoryDeEquipPlugin(openTibiaId, new LuaScriptingInventoryDeEquipPlugin(script, parameters) );
        }

        public InventoryDeEquipPlugin GetInventoryDeEquipPlugin(ushort openTibiaId)
        {
            return inventoryDeEquipPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<string, PlayerSayPlugin> playerSayPlugins = new PluginDictionaryCached<string, PlayerSayPlugin>();

        private void AddPlayerSayPlugin(string message, PlayerSayPlugin playerSayPlugin)
        {
            playerSayPlugins.AddPlugin(message, playerSayPlugin);
        }

        public void AddPlayerSayPlugin(string message, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerSayPlugin(message, new LuaScriptingPlayerSayPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerSayPlugin(message, (PlayerSayPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerSayPlugin(string message, ILuaScope script, LuaTable parameters)
        {
            AddPlayerSayPlugin(message, new LuaScriptingPlayerSayPlugin(script, parameters) );
        }

        public PlayerSayPlugin GetPlayerSayPlugin(string message)
        {
            return playerSayPlugins.GetPlugin(message);
        }

        private PluginListCached<PlayerLoginPlugin> playerLoginPlugins = new PluginListCached<PlayerLoginPlugin>();

        private void AddPlayerLoginPlugin(PlayerLoginPlugin playerLoginPlugin)
        {
            playerLoginPlugins.AddPlugin(playerLoginPlugin);
        }

        public void AddPlayerLoginPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerLoginPlugin(new LuaScriptingPlayerLoginPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerLoginPlugin( (PlayerLoginPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerLoginPlugin(ILuaScope script, LuaTable parameters)
        {
            AddPlayerLoginPlugin(new LuaScriptingPlayerLoginPlugin(script, parameters) );
        }

        public IEnumerable<PlayerLoginPlugin> GetPlayerLoginPlugins()
        {
            return playerLoginPlugins.GetPlugins();
        }

        private PluginListCached<PlayerLogoutPlugin> playerLogoutPlugins = new PluginListCached<PlayerLogoutPlugin>();

        private void AddPlayerLogoutPlugin(PlayerLogoutPlugin playerLogoutPlugin)
        {
            playerLogoutPlugins.AddPlugin(playerLogoutPlugin);
        }

        public void AddPlayerLogoutPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerLogoutPlugin(new LuaScriptingPlayerLogoutPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerLogoutPlugin( (PlayerLogoutPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerLogoutPlugin(ILuaScope script, LuaTable parameters)
        {
            AddPlayerLogoutPlugin(new LuaScriptingPlayerLogoutPlugin(script, parameters) );
        }

        public IEnumerable<PlayerLogoutPlugin> GetPlayerLogoutPlugins()
        {
            return playerLogoutPlugins.GetPlugins();
        }
        
        private PluginListCached<PlayerAdvanceLevelPlugin> playerAdvanceLevelPlugins = new PluginListCached<PlayerAdvanceLevelPlugin>();

        private void AddPlayerAdvanceLevelPlugin(PlayerAdvanceLevelPlugin playerAdvanceLevelPlugin)
        {
            playerAdvanceLevelPlugins.AddPlugin(playerAdvanceLevelPlugin);
        }

        public void AddPlayerAdvanceLevelPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerAdvanceLevelPlugin(new LuaScriptingPlayerAdvanceLevelPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerAdvanceLevelPlugin( (PlayerAdvanceLevelPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerAdvanceLevelPlugin(ILuaScope script, LuaTable parameters)
        {
            AddPlayerAdvanceLevelPlugin(new LuaScriptingPlayerAdvanceLevelPlugin(script, parameters) );
        }

        public IEnumerable<PlayerAdvanceLevelPlugin> GetPlayerAdvanceLevelPlugins()
        {
            return playerAdvanceLevelPlugins.GetPlugins();
        }

        private PluginListCached<PlayerAdvanceSkillPlugin> playerAdvanceSkillPlugins = new PluginListCached<PlayerAdvanceSkillPlugin>();

        private void AddPlayerAdvanceSkillPlugin(PlayerAdvanceSkillPlugin playerAdvanceSkillPlugin)
        {
            playerAdvanceSkillPlugins.AddPlugin(playerAdvanceSkillPlugin);
        }

        public void AddPlayerAdvanceSkillPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerAdvanceSkillPlugin(new LuaScriptingPlayerAdvanceSkillPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerAdvanceSkillPlugin( (PlayerAdvanceSkillPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerAdvanceSkillPlugin(ILuaScope script, LuaTable parameters)
        {
            AddPlayerAdvanceSkillPlugin(new LuaScriptingPlayerAdvanceSkillPlugin(script, parameters) );
        }

        public IEnumerable<PlayerAdvanceSkillPlugin> GetPlayerAdvanceSkillPlugins()
        {
            return playerAdvanceSkillPlugins.GetPlugins();
        }

        private PluginListCached<CreatureDeathPlugin> creatureDeathPlugins = new PluginListCached<CreatureDeathPlugin>();

        private void AddCreatureDeathPlugin(CreatureDeathPlugin creatureDeathPlugin)
        {
            creatureDeathPlugins.AddPlugin(creatureDeathPlugin);
        }

        public void AddCreatureDeathPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddCreatureDeathPlugin(new LuaScriptingCreatureDeathPlugin(fileName, parameters) );
            }
            else
            {
                AddCreatureDeathPlugin( (CreatureDeathPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddCreatureDeathPlugin(ILuaScope script, LuaTable parameters)
        {
            AddCreatureDeathPlugin(new LuaScriptingCreatureDeathPlugin(script, parameters) );
        }

        public IEnumerable<CreatureDeathPlugin> GetCreatureDeathPlugins()
        {
            return creatureDeathPlugins.GetPlugins();
        }

        private PluginListCached<PlayerEarnAchievementPlugin> playerEarnAchievementPlugins = new PluginListCached<PlayerEarnAchievementPlugin>();

        private void AddPlayerEarnAchievementPlugin(PlayerEarnAchievementPlugin playerEarnAchievementPlugin)
        {
            playerEarnAchievementPlugins.AddPlugin(playerEarnAchievementPlugin);
        }

        public void AddPlayerEarnAchievementPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerEarnAchievementPlugin(new LuaScriptingPlayerEarnAchievementPlugin(fileName, parameters) );
            }
            else
            {
                AddPlayerEarnAchievementPlugin( (PlayerEarnAchievementPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerEarnAchievementPlugin(ILuaScope script, LuaTable parameters)
        {
            AddPlayerEarnAchievementPlugin(new LuaScriptingPlayerEarnAchievementPlugin(script, parameters) );
        }

        public IEnumerable<PlayerEarnAchievementPlugin> GetPlayerEarnAchievementPlugins()
        {
            return playerEarnAchievementPlugins.GetPlugins();
        }

        private PluginListCached<ServerStartupPlugin> serverStartupPlugins = new PluginListCached<ServerStartupPlugin>();

        private void AddServerStartupPlugin(ServerStartupPlugin serverStartupPlugin)
        {
            serverStartupPlugins.AddPlugin(serverStartupPlugin);
        }

        public void AddServerStartupPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddServerStartupPlugin(new LuaScriptingServerStartupPlugin(fileName, parameters) );
            }
            else
            {
                AddServerStartupPlugin( (ServerStartupPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddServerStartupPlugin(ILuaScope script, LuaTable parameters)
        {
            AddServerStartupPlugin(new LuaScriptingServerStartupPlugin(script, parameters) );
        }

        public IEnumerable<ServerStartupPlugin> GetServerStartupPlugins()
        {
            return serverStartupPlugins.GetPlugins();
        }

        private PluginListCached<ServerShutdownPlugin> serverShutdownPlugins = new PluginListCached<ServerShutdownPlugin>();

        private void AddServerShutdownPlugin(ServerShutdownPlugin serverShutdownPlugin)
        {
            serverShutdownPlugins.AddPlugin(serverShutdownPlugin);
        }

        public void AddServerShutdownPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddServerShutdownPlugin(new LuaScriptingServerShutdownPlugin(fileName, parameters) );
            }
            else
            {
                AddServerShutdownPlugin( (ServerShutdownPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddServerShutdownPlugin(ILuaScope script, LuaTable parameters)
        {
            AddServerShutdownPlugin(new LuaScriptingServerShutdownPlugin(script, parameters) );
        }

        public IEnumerable<ServerShutdownPlugin> GetServerShutdownPlugins()
        {
            return serverShutdownPlugins.GetPlugins();
        }

        private PluginListCached<ServerSavePlugin> serverSavePlugins = new PluginListCached<ServerSavePlugin>();

        private void AddServerSavePlugin(ServerSavePlugin serverSavePlugin)
        {
            serverSavePlugins.AddPlugin(serverSavePlugin);
        }

        public void AddServerSavePlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddServerSavePlugin(new LuaScriptingServerSavePlugin(fileName, parameters) );
            }
            else
            {
                AddServerSavePlugin( (ServerSavePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddServerSavePlugin(ILuaScope script, LuaTable parameters)
        {
            AddServerSavePlugin(new LuaScriptingServerSavePlugin(script, parameters) );
        }

        public IEnumerable<ServerSavePlugin> GetServerSavePlugins()
        {
            return serverSavePlugins.GetPlugins();
        }

        private PluginListCached<ServerRecordPlugin> serverRecordPlugins = new PluginListCached<ServerRecordPlugin>();

        private void AddServerRecordPlugin(ServerRecordPlugin serverRecordPlugin)
        {
            serverRecordPlugins.AddPlugin(serverRecordPlugin);
        }

        public void AddServerRecordPlugin(string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddServerRecordPlugin(new LuaScriptingServerRecordPlugin(fileName, parameters) );
            }
            else
            {
                AddServerRecordPlugin( (ServerRecordPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddServerRecordPlugin(ILuaScope script, LuaTable parameters)
        {
            AddServerRecordPlugin(new LuaScriptingServerRecordPlugin(script, parameters) );
        }

        public IEnumerable<ServerRecordPlugin> GetServerRecordPlugins()
        {
            return serverRecordPlugins.GetPlugins();
        }

        private PluginDictionary<string, DialoguePlugin> dialoguePlugins = new PluginDictionary<string, DialoguePlugin>();

        private void AddDialoguePlugin(string name, Func<DialoguePlugin> dialoguePlugin)
        {
            dialoguePlugins.AddPlugin(name, dialoguePlugin);
        }

        public void AddDialoguePlugin(string name, string fileName, LuaTable parameters)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddDialoguePlugin(name, () => new LuaScriptingDialoguePlugin(fileName, parameters) );
            }
            else
            {
                AddDialoguePlugin(name, () => (DialoguePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddDialoguePlugin(string name, ILuaScope script, LuaTable parameters)
        {
            AddDialoguePlugin(name, () => new LuaScriptingDialoguePlugin(script, parameters) );
        }

        public DialoguePlugin GetDialoguePlugin(string name)
        {
            return dialoguePlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<string, SpellPlugin> spellPluginsRequiresTarget = new PluginDictionaryCached<string, SpellPlugin>();
        private PluginDictionaryCached<string, SpellPlugin> spellPlugins = new PluginDictionaryCached<string, SpellPlugin>();

        private void AddSpellPlugin(bool requiresTarget, SpellPlugin spellPlugin)
        {
            spells.Add(spellPlugin.Spell);

            if (requiresTarget)
            {
                spellPluginsRequiresTarget.AddPlugin(spellPlugin.Spell.Words, spellPlugin);
            }
            else
            {
                spellPlugins.AddPlugin(spellPlugin.Spell.Words, spellPlugin);
            }
        }

        public void AddSpellPlugin(bool requiresTarget, string fileName, LuaTable parameters, Spell spell)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddSpellPlugin(requiresTarget, new LuaScriptingSpellPlugin(fileName, parameters, spell) );
            }
            else
            {
                AddSpellPlugin(requiresTarget, (SpellPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), spell) );
            }
        }

        public void AddSpellPlugin(bool requiresTarget, ILuaScope script, LuaTable parameters, Spell spell)
        {
            AddSpellPlugin(requiresTarget, new LuaScriptingSpellPlugin(script, parameters, spell) );
        }

        public SpellPlugin GetSpellPlugin(bool requiresTarget, string words)
        {
            if (requiresTarget)
            {
                return spellPluginsRequiresTarget.GetPlugin(words);
            }
            else
            {
                return spellPlugins.GetPlugin(words);
            }
        }

        private PluginDictionaryCached<ushort, RunePlugin> runePluginsRequiresTarget = new PluginDictionaryCached<ushort, RunePlugin>();
        private PluginDictionaryCached<ushort, RunePlugin> runePlugins = new PluginDictionaryCached<ushort, RunePlugin>();

        private void AddRunePlugin(bool requiresTarget, RunePlugin runePlugin)
        {
            runes.Add(runePlugin.Rune);

            if (requiresTarget)
            {
                runePluginsRequiresTarget.AddPlugin(runePlugin.Rune.OpenTibiaId, runePlugin);
            }
            else
            {
                runePlugins.AddPlugin(runePlugin.Rune.OpenTibiaId, runePlugin);
            }
        }

        public void AddRunePlugin(bool requiresTarget, string fileName, LuaTable parameters, Rune rune)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddRunePlugin(requiresTarget, new LuaScriptingRunePlugin(fileName, parameters, rune) );
            }
            else
            {
                AddRunePlugin(requiresTarget, (RunePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), rune) );
            }
        }

        public void AddRunePlugin(bool requiresTarget, ILuaScope script, LuaTable parameters, Rune rune)
        {
            AddRunePlugin(requiresTarget, new LuaScriptingRunePlugin(script, parameters, rune) );
        }

        public RunePlugin GetRunePlugin(bool requiresTarget, ushort openTibiaId)
        {
            if (requiresTarget)
            {
                return runePluginsRequiresTarget.GetPlugin(openTibiaId);
            }
            else
            {
                return runePlugins.GetPlugin(openTibiaId);
            }
        }

        private PluginDictionaryCached<ushort, WeaponPlugin> weaponPlugins = new PluginDictionaryCached<ushort, WeaponPlugin>();

        private void AddWeaponPlugin(WeaponPlugin weaponPlugin)
        {
            weapons.Add(weaponPlugin.Weapon);

            weaponPlugins.AddPlugin(weaponPlugin.Weapon.OpenTibiaId, weaponPlugin);
        }

        public void AddWeaponPlugin(string fileName, LuaTable parameters, Weapon weapon)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddWeaponPlugin(new LuaScriptingWeaponPlugin(fileName, parameters, weapon) );
            }
            else
            {
                AddWeaponPlugin( (WeaponPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), weapon) );
            }
        }

        public void AddWeaponPlugin(ILuaScope script, LuaTable parameters, Weapon weapon)
        {
            AddWeaponPlugin(new LuaScriptingWeaponPlugin(script, parameters, weapon) );
        }

        public WeaponPlugin GetWeaponPlugin(ushort openTibiaId)
        {
            return weaponPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, AmmunitionPlugin> ammunitionPlugins = new PluginDictionaryCached<ushort, AmmunitionPlugin>();

        private void AddAmmunitionPlugin(AmmunitionPlugin ammunitionPlugin)
        {
            ammunitions.Add(ammunitionPlugin.Ammunition);

            ammunitionPlugins.AddPlugin(ammunitionPlugin.Ammunition.OpenTibiaId, ammunitionPlugin);
        }

        public void AddAmmunitionPlugin(string fileName, LuaTable parameters, Ammunition ammunition)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddAmmunitionPlugin(new LuaScriptingAmmunitionPlugin(fileName, parameters, ammunition) );
            }
            else
            {
                AddAmmunitionPlugin( (AmmunitionPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), ammunition) );
            }
        }

        public void AddAmmunitionPlugin(ILuaScope script, LuaTable parameters, Ammunition ammunition)
        {
            AddAmmunitionPlugin(new LuaScriptingAmmunitionPlugin(script, parameters, ammunition) );
        }

        public AmmunitionPlugin GetAmmunitionPlugin(ushort openTibiaId)
        {
            return ammunitionPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<string, RaidPlugin> raidPlugins = new PluginDictionaryCached<string, RaidPlugin>();

        private void AddRaidPlugin(RaidPlugin raidPlugin)
        {
            raids.Add(raidPlugin.Raid);

            raidPlugins.AddPlugin(raidPlugin.Raid.Name, raidPlugin);
        }

        public void AddRaidPlugin(string fileName, LuaTable parameters, Raid raid)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddRaidPlugin(new LuaScriptingRaidPlugin(fileName, parameters, raid) );
            }
            else
            {
                AddRaidPlugin( (RaidPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), raid) );
            }
        }

        public void AddRaidPlugin(ILuaScope script, LuaTable parameters, Raid raid)
        {
            AddRaidPlugin(new LuaScriptingRaidPlugin(script, parameters, raid) );
        }

        public RaidPlugin GetRaidPlugin(string name)
        {
            return raidPlugins.GetPlugin(name);
        }

        private List<Spell> spells = new List<Spell>();

        public List<Spell> Spells
        {
            get 
            {
                return spells; 
            }
        }

        private List<Rune> runes = new List<Rune>();

        public List<Rune> Runes
        {
            get
            {
                return runes;
            }
        }

        private List<Weapon> weapons = new List<Weapon>();

        public List<Weapon> Weapons
        {
            get
            {
                return weapons;
            }
        }

        private List<Ammunition> ammunitions = new List<Ammunition>();

        public List<Ammunition> Ammunitions
        {
            get
            {
                return ammunitions;
            }
        }

        private List<Raid> raids = new List<Raid>();

        public List<Raid> Raids
        {
            get
            {
                return raids;
            }
        }

        public void Stop()
        {
            var pluginLists = new IEnumerable<Plugin>[]
            {
                playerRotateItemPlugins.GetPlugins(),                  
                   
                playerUseItemPlugins.GetPlugins(),

                playerUseItemWithCreaturePluginsAllowFarUse.GetPlugins(),

                playerUseItemWithCreaturePlugins.GetPlugins(),

                playerUseItemWithItemPluginsAllowFarUse.GetPlugins(),

                playerUseItemWithItemPlugins.GetPlugins(),

                playerMoveCreaturePlugins.GetPlugins(),

                playerMoveItemPlugins.GetPlugins(),

                creatureStepInPlugins.GetPlugins(),

                creatureStepOutPlugins.GetPlugins(),

                inventoryEquipPlugins.GetPlugins(),

                inventoryDeEquipPlugins.GetPlugins(),

                playerSayPlugins.GetPlugins(),

                playerLoginPlugins.GetPlugins(),

                playerLogoutPlugins.GetPlugins(),

                playerAdvanceLevelPlugins.GetPlugins(),

                playerAdvanceSkillPlugins.GetPlugins(),

                creatureDeathPlugins.GetPlugins(),

                playerEarnAchievementPlugins.GetPlugins(),

                serverStartupPlugins.GetPlugins(),

                serverSavePlugins.GetPlugins(),

                serverRecordPlugins.GetPlugins(),

                serverShutdownPlugins.GetPlugins(),

                dialoguePlugins.GetPlugins(),

                spellPluginsRequiresTarget.GetPlugins(),

                spellPlugins.GetPlugins(),

                runePluginsRequiresTarget.GetPlugins(),

                runePlugins.GetPlugins(),

                weaponPlugins.GetPlugins(),

                ammunitionPlugins.GetPlugins(),

                raidPlugins.GetPlugins()
            };

            foreach (var pluginList in pluginLists)
            {
                foreach (var plugin in pluginList)
                {
                    plugin.Stop();
                }
            }
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
                    if (autoLoadPlugins != null)
                    {
                        foreach (var autoLoadPlugin in autoLoadPlugins)
                        {
                            autoLoadPlugin.Dispose();
                        }
                    }

                    if (script != null)
                    {
                        script.Dispose();
                    }
                }
            }
        }
    }
}