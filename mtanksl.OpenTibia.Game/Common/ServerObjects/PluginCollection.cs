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
            private LuaScope script;

            public AutoLoadPlugin(PluginCollection pluginCollection, string filePath)
            {
                LuaScope scripts = pluginCollection.server.LuaScripts.LoadLib(
                    pluginCollection.server.PathResolver.GetFullPath("data/plugins/scripts/lib.lua"), 
                    pluginCollection.server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    pluginCollection.server.PathResolver.GetFullPath("data/lib.lua") );

                var initializations = new List<(string Type, LuaTable Parameters)>();

                scripts["registerplugin"] = (string type, LuaTable parameters) => 
                {
                    initializations.Add( (type, parameters) );
                };

                script = pluginCollection.server.LuaScripts.LoadScript(pluginCollection.server.PathResolver.GetFullPath(filePath), scripts);

                foreach (var initialization in initializations)
                {
                    if (initialization.Type == "actions")
                    {
                        string type = (string)initialization.Parameters["type"];

                        switch (type)
                        {
                            case "PlayerRotateItem":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                pluginCollection.AddPlayerRotateItemPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "PlayerUseItem":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                pluginCollection.AddPlayerUseItemPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "PlayerUseItemWithItem":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                bool allowFarUse = (bool)initialization.Parameters["allowfaruse"];

                                pluginCollection.AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, script, initialization.Parameters);                     
                            }
                            break;

                            case "PlayerUseItemWithCreature":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                bool allowFarUse = (bool)initialization.Parameters["allowfaruse"];

                                pluginCollection.AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "PlayerMoveItem":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                pluginCollection.AddPlayerMoveItemPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "PlayerMoveCreature":
                            {
                                string name = (string)initialization.Parameters["name"];

                                pluginCollection.AddPlayerMoveCreaturePlugin(name, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "movements")
                    {
                        string type = (string)initialization.Parameters["type"];

                        switch (type)
                        {
                            case "CreatureStepIn":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                pluginCollection.AddCreatureStepInPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "CreatureStepOut":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                pluginCollection.AddCreatureStepOutPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "InventoryEquip":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                pluginCollection.AddInventoryEquipPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;

                            case "InventoryDeEquip":
                            {
                                ushort openTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"];

                                pluginCollection.AddInventoryDeEquipPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "talkactions")
                    {
                        string type = (string)initialization.Parameters["type"];

                        switch (type)
                        {
                            case "PlayerSay":
                            {
                                string message = (string)initialization.Parameters["message"];

                                pluginCollection.AddPlayerSayPlugin(message, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "creaturescripts")
                    {
                        string type = (string)initialization.Parameters["type"];

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
                        }
                    }
                    else if (initialization.Type == "npcs")
                    {
                        string type = (string)initialization.Parameters["type"];

                        switch (type)
                        {
                            case "Dialogue":
                            {
                                string name = (string)initialization.Parameters["name"];

                                pluginCollection.AddDialoguePlugin(name, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "spells")
                    {
                        bool requiresTarget = (bool)initialization.Parameters["requirestarget"];

                        Spell spell = new Spell()
                        {
                            Words = (string)initialization.Parameters["words"],

                            Name = (string)initialization.Parameters["name"],

                            Group = (string)initialization.Parameters["group"],

                            Cooldown = TimeSpan.FromSeconds( (int)(long)initialization.Parameters["cooldown"] ),

                            GroupCooldown = TimeSpan.FromSeconds( (int)(long)initialization.Parameters["groupcooldown"] ),

                            Level = (int)(long)initialization.Parameters["level"],

                            Mana = (int)(long)initialization.Parameters["mana"],

                            Soul = (int)(long)initialization.Parameters["soul"],

                            ConjureOpenTibiaId = (ushort?)(long?)initialization.Parameters["conjureopentibiaid"],

                            ConjureCount = (int?)(long?)initialization.Parameters["conjurecount"],

                            Premium = (bool)initialization.Parameters["premium"],

                            Vocations = ( (LuaTable)initialization.Parameters["vocations"]).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                        };

                        pluginCollection.AddSpellPlugin(requiresTarget, script, initialization.Parameters, spell);
                    }
                    else if (initialization.Type == "runes")
                    {
                        bool requiresTarget = (bool)initialization.Parameters["requirestarget"];

                        Rune rune = new Rune()
                        {
                            OpenTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"],

                            Name = (string)initialization.Parameters["name"],

                            Group = (string)initialization.Parameters["group"],

                            GroupCooldown = TimeSpan.FromSeconds( (int)(long)initialization.Parameters["groupcooldown"]),

                            Level = (int)(long)initialization.Parameters["level"],

                            MagicLevel = (int)(long)initialization.Parameters["magiclevel"]
                        };

                        pluginCollection.AddRunePlugin(requiresTarget, script, initialization.Parameters, rune);
                    }
                    else if (initialization.Type == "weapons")
                    {
                        Weapon weapon = new Weapon()
                        {
                            OpenTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"],

                            Level = (int)(long)initialization.Parameters["level"],

                            Mana = (int)(long)initialization.Parameters["mana"],

                            Vocations = ( (LuaTable)initialization.Parameters["vocations"]).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                        };

                        pluginCollection.AddWeaponPlugin( script, initialization.Parameters, weapon);
                    }
                    else if (initialization.Type == "ammunitions")
                    {
                        Ammunition ammunition = new Ammunition()
                        {
                            OpenTibiaId = (ushort)(long)initialization.Parameters["opentibiaid"]
                        };

                        pluginCollection.AddAmmunitionPlugin(script, initialization.Parameters, ammunition);
                    }
                }
            }

            public void Dispose()
            {
                script.Dispose();
            }
        }

        private IServer server;

        public PluginCollection(IServer server)
        {
            this.server = server;
        }

        private LuaScope script;

        private List<AutoLoadPlugin> autoLoadPlugins = new List<AutoLoadPlugin>();

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/plugins/config.lua"), 
                server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                server.PathResolver.GetFullPath("data/lib.lua") );

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.actions"] ).Values)
            {
                string type = (string)plugin["type"];

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "PlayerRotateItem":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        AddPlayerRotateItemPlugin(openTibiaId, fileName);
                    }
                    break;

                    case "PlayerUseItem":
                    { 
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        AddPlayerUseItemPlugin(openTibiaId, fileName);
                    }
                    break;

                    case "PlayerUseItemWithItem":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        bool allowFarUse = (bool)plugin["allowfaruse"];

                        AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, fileName);
                    }
                    break;

                    case "PlayerUseItemWithCreature":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        bool allowFarUse = (bool)plugin["allowfaruse"];

                        AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, fileName);
                    }
                    break;

                    case "PlayerMoveItem":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        AddPlayerMoveItemPlugin(openTibiaId, fileName);
                    }
                    break;

                    case "PlayerMoveCreature":
                    {
                        string name = (string)plugin["name"];

                        AddPlayerMoveCreaturePlugin(name, fileName);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.movements"] ).Values)
            {
                string type = (string)plugin["type"];

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "CreatureStepIn":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        AddCreatureStepInPlugin(openTibiaId, fileName);
                    }
                    break;

                    case "CreatureStepOut":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        AddCreatureStepOutPlugin(openTibiaId, fileName);
                    }
                    break;

                    case "InventoryEquip":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        AddInventoryEquipPlugin(openTibiaId, fileName);
                    }
                    break;

                    case "InventoryDeEquip":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        AddInventoryDeEquipPlugin(openTibiaId, fileName);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.talkactions"] ).Values)
            {
                string type = (string)plugin["type"];

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "PlayerSay":
                    {
                        string message = (string)plugin["message"];

                        AddPlayerSayPlugin(message, fileName);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.creaturescripts"] ).Values)
            {
                string type = (string)plugin["type"];

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "PlayerLogin":
                    {
                        AddPlayerLoginPlugin(fileName);
                    }
                    break;

                    case "PlayerLogout":
                    {
                        AddPlayerLogoutPlugin(fileName);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.npcs"] ).Values)
            {
                string type = (string)plugin["type"];

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "Dialogue":
                    {
                        string name = (string)plugin["name"];

                        AddDialoguePlugin(name, fileName);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.spells"] ).Values)
            {
                string fileName = (string)plugin["filename"];

                bool requiresTarget = (bool)plugin["requirestarget"];

                Spell spell = new Spell()
                {
                    Words = (string)plugin["words"],

                    Name = (string)plugin["name"],

                    Group = (string)plugin["group"],

                    Cooldown = TimeSpan.FromSeconds( (int)(long)plugin["cooldown"]),

                    GroupCooldown = TimeSpan.FromSeconds( (int)(long)plugin["groupcooldown"]),

                    Level = (int)(long)plugin["level"],

                    Mana = (int)(long)plugin["mana"],

                    Soul = (int)(long)plugin["soul"],

                    ConjureOpenTibiaId = (ushort?)(long?)plugin["conjureopentibiaid"],

                    ConjureCount = (int?)(long?)plugin["conjurecount"],

                    Premium = (bool)plugin["premium"],

                    Vocations = ( (LuaTable)plugin["vocations"]).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                };

                AddSpellPlugin(requiresTarget, fileName, spell);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.runes"] ).Values)
            {
                string fileName = (string)plugin["filename"];

                bool requiresTarget = (bool)plugin["requirestarget"];

                Rune rune = new Rune()
                {
                    OpenTibiaId = (ushort)(long)plugin["opentibiaid"],

                    Name = (string)plugin["name"],

                    Group = (string)plugin["group"],

                    GroupCooldown = TimeSpan.FromSeconds( (int)(long)plugin["groupcooldown"]),

                    Level = (int)(long)plugin["level"],

                    MagicLevel = (int)(long)plugin["magiclevel"]
                };

                AddRunePlugin(requiresTarget, fileName, rune);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.weapons"] ).Values)
            {
                string fileName = (string)plugin["filename"];

                Weapon weapon = new Weapon()
                {
                    OpenTibiaId = (ushort)(long)plugin["opentibiaid"],

                    Level = (int)(long)plugin["level"],

                    Mana = (int)(long)plugin["mana"],

                    Vocations = ( (LuaTable)plugin["vocations"]).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                };

                AddWeaponPlugin(fileName, weapon);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.ammunitions"] ).Values)
            {
                string fileName = (string)plugin["filename"];

                Ammunition ammunition = new Ammunition()
                {
                    OpenTibiaId = (ushort)(long)plugin["opentibiaid"]
                };

                AddAmmunitionPlugin(fileName, ammunition);
            }

            foreach (var filePath in Directory.GetFiles(server.PathResolver.GetFullPath("data/plugins/scripts"), "*.lua", SearchOption.AllDirectories) )
            {
                string fileName = Path.GetFileName(filePath);

                if (fileName != "lib.lua")
                {
                    autoLoadPlugins.Add(new AutoLoadPlugin(this, filePath) );
                }
            }

            LuaScope scripts;

            if (server.LuaScripts.TryGetLib(server.PathResolver.GetFullPath("data/plugins/scripts/lib.lua"), out scripts) )
            {
                scripts["registerplugin"] = null;
            }
        }

        public object GetValue(string key)
        {
            return script[key];
        }

        private Dictionary<string, Plugin> plugins = new Dictionary<string, Plugin>();

        private T GetPlugin<T>(string fileName, Func<T> factory) where T: Plugin
        {
            Plugin plugin;

            if ( !plugins.TryGetValue(fileName, out plugin) )
            {
                plugin = factory();

                plugins.Add(fileName, plugin);
            }

            return (T)plugin;
        }

        private PluginDictionaryCached<ushort, PlayerRotateItemPlugin> playerRotateItemPlugins = new PluginDictionaryCached<ushort, PlayerRotateItemPlugin>();

        public void AddPlayerRotateItemPlugin(ushort openTibiaId, PlayerRotateItemPlugin playerRotateItemPlugin)
        {
            playerRotateItemPlugins.AddPlugin(openTibiaId, playerRotateItemPlugin);
        }

        public void AddPlayerRotateItemPlugin(ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerRotateItemPlugin(openTibiaId, new LuaScriptingPlayerRotateItemPlugin(fileName) );
            }
            else
            {
                AddPlayerRotateItemPlugin(openTibiaId, (PlayerRotateItemPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerRotateItemPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters)
        {
            AddPlayerRotateItemPlugin(openTibiaId, new LuaScriptingPlayerRotateItemPlugin(script, parameters) );
        }

        public PlayerRotateItemPlugin GetPlayerRotateItemPlugin(ushort openTibiaId)
        {
            return playerRotateItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, PlayerUseItemPlugin> playerUseItemPlugins = new PluginDictionaryCached<ushort, PlayerUseItemPlugin>();

        public void AddPlayerUseItemPlugin(ushort openTibiaId, PlayerUseItemPlugin playerUseItemPlugin)
        {
            playerUseItemPlugins.AddPlugin(openTibiaId, playerUseItemPlugin);
        }

        public void AddPlayerUseItemPlugin(ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerUseItemPlugin(openTibiaId, new LuaScriptingPlayerUseItemPlugin(fileName) );
            }
            else
            {
                AddPlayerUseItemPlugin(openTibiaId, (PlayerUseItemPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerUseItemPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters)
        {
            AddPlayerUseItemPlugin(openTibiaId, new LuaScriptingPlayerUseItemPlugin(script, parameters) );
        }

        public PlayerUseItemPlugin GetPlayerUseItemPlugin(ushort openTibiaId)
        {
            return playerUseItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin> playerUseItemWithItemPluginsAllowFarUse = new PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin>();
        private PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin> playerUseItemWithItemPlugins = new PluginDictionaryCached<ushort, PlayerUseItemWithItemPlugin>();

        public void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, PlayerUseItemWithItemPlugin playerUseItemWithItemPlugin)
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
           
        public void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, new LuaScriptingPlayerUseItemWithItemPlugin(fileName) );
            }
            else
            {
                AddPlayerUseItemWithItemPlugin(allowFarUse, openTibiaId, (PlayerUseItemWithItemPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, LuaScope script, LuaTable parameters)
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

        public void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, PlayerUseItemWithCreaturePlugin playerUseItemWithCreaturePlugin)
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
        
        public void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, new LuaScriptingPlayerUseItemWithCreaturePlugin(fileName) );
            }
            else
            {
                AddPlayerUseItemWithCreaturePlugin(allowFarUse, openTibiaId, (PlayerUseItemWithCreaturePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, LuaScope script, LuaTable parameters)
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

        public void AddPlayerMoveCreaturePlugin(string name, PlayerMoveCreaturePlugin playerMoveCreaturePlugin)
        {
            playerMoveCreaturePlugins.AddPlugin(name, playerMoveCreaturePlugin);
        }

        public void AddPlayerMoveCreaturePlugin(string name, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerMoveCreaturePlugin(name, new LuaScriptingPlayerMoveCreaturePlugin(fileName) );
            }
            else
            {
                AddPlayerMoveCreaturePlugin(name, (PlayerMoveCreaturePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerMoveCreaturePlugin(string name, LuaScope script, LuaTable parameters)
        {
            AddPlayerMoveCreaturePlugin(name, new LuaScriptingPlayerMoveCreaturePlugin(script, parameters) );
        }

        public PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(string name)
        {
            return playerMoveCreaturePlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<ushort, PlayerMoveItemPlugin> playerMoveItemPlugins = new PluginDictionaryCached<ushort, PlayerMoveItemPlugin>();

        public void AddPlayerMoveItemPlugin(ushort openTibiaId, PlayerMoveItemPlugin playerMoveItemPlugin)
        {
            playerMoveItemPlugins.AddPlugin(openTibiaId, playerMoveItemPlugin);
        }

        public void AddPlayerMoveItemPlugin(ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerMoveItemPlugin(openTibiaId, new LuaScriptingPlayerMoveItemPlugin(fileName) );
            }
            else
            {
                AddPlayerMoveItemPlugin(openTibiaId, (PlayerMoveItemPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerMoveItemPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters)
        {
            AddPlayerMoveItemPlugin(openTibiaId, new LuaScriptingPlayerMoveItemPlugin(script, parameters) );
        }

        public PlayerMoveItemPlugin GetPlayerMoveItemPlugin(ushort openTibiaId)
        {
            return playerMoveItemPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, CreatureStepInPlugin> creatureStepInPlugins = new PluginDictionaryCached<ushort, CreatureStepInPlugin>();

        public void AddCreatureStepInPlugin(ushort openTibiaId, CreatureStepInPlugin creatureStepInPlugin)
        {
            creatureStepInPlugins.AddPlugin(openTibiaId, creatureStepInPlugin);
        }
                    
        public void AddCreatureStepInPlugin(ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddCreatureStepInPlugin(openTibiaId, new LuaScriptingCreatureStepInPlugin(fileName) );
            }
            else
            {
                AddCreatureStepInPlugin(openTibiaId, (CreatureStepInPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddCreatureStepInPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters)
        {
            AddCreatureStepInPlugin(openTibiaId, new LuaScriptingCreatureStepInPlugin(script, parameters) );
        }

        public CreatureStepInPlugin GetCreatureStepInPlugin(ushort openTibiaId)
        {
            return creatureStepInPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, CreatureStepOutPlugin> creatureStepOutPlugins = new PluginDictionaryCached<ushort, CreatureStepOutPlugin>();

        public void AddCreatureStepOutPlugin(ushort openTibiaId, CreatureStepOutPlugin creatureStepOutPlugin)
        {
            creatureStepOutPlugins.AddPlugin(openTibiaId, creatureStepOutPlugin);
        }

        public void AddCreatureStepOutPlugin(ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddCreatureStepOutPlugin(openTibiaId, new LuaScriptingCreatureStepOutPlugin(fileName) );
            }
            else
            {
                AddCreatureStepOutPlugin(openTibiaId, (CreatureStepOutPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddCreatureStepOutPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters)
        {
            AddCreatureStepOutPlugin(openTibiaId, new LuaScriptingCreatureStepOutPlugin(script, parameters));
        }

        public CreatureStepOutPlugin GetCreatureStepOutPlugin(ushort openTibiaId)
        {
            return creatureStepOutPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, InventoryEquipPlugin> inventoryEquipPlugins = new PluginDictionaryCached<ushort, InventoryEquipPlugin>();

        public void AddInventoryEquipPlugin(ushort openTibiaId, InventoryEquipPlugin inventoryEquipPlugin)
        {
            inventoryEquipPlugins.AddPlugin(openTibiaId, inventoryEquipPlugin);
        }

        public void AddInventoryEquipPlugin(ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddInventoryEquipPlugin(openTibiaId, new LuaScriptingInventoryEquipPlugin(fileName) );
            }
            else
            {
                AddInventoryEquipPlugin(openTibiaId, (InventoryEquipPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddInventoryEquipPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters)
        {
            AddInventoryEquipPlugin(openTibiaId, new LuaScriptingInventoryEquipPlugin(script, parameters) );
        }

        public InventoryEquipPlugin GetInventoryEquipPlugin(ushort openTibiaId)
        {
            return inventoryEquipPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, InventoryDeEquipPlugin> inventoryDeEquipPlugins = new PluginDictionaryCached<ushort, InventoryDeEquipPlugin>();

        public void AddInventoryDeEquipPlugin(ushort openTibiaId, InventoryDeEquipPlugin inventoryDeEquipPlugin)
        {
            inventoryDeEquipPlugins.AddPlugin(openTibiaId, inventoryDeEquipPlugin);
        }

        public void AddInventoryDeEquipPlugin(ushort openTibiaId, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddInventoryDeEquipPlugin(openTibiaId, new LuaScriptingInventoryDeEquipPlugin(fileName) );
            }
            else
            {
                AddInventoryDeEquipPlugin(openTibiaId, (InventoryDeEquipPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddInventoryDeEquipPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters)
        {
            AddInventoryDeEquipPlugin(openTibiaId, new LuaScriptingInventoryDeEquipPlugin(script, parameters) );
        }

        public InventoryDeEquipPlugin GetInventoryDeEquipPlugin(ushort openTibiaId)
        {
            return inventoryDeEquipPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<string, PlayerSayPlugin> playerSayPlugins = new PluginDictionaryCached<string, PlayerSayPlugin>();

        public void AddPlayerSayPlugin(string message, PlayerSayPlugin playerSayPlugin)
        {
            playerSayPlugins.AddPlugin(message, playerSayPlugin);
        }

        public void AddPlayerSayPlugin(string message, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerSayPlugin(message, new LuaScriptingPlayerSayPlugin(fileName) );
            }
            else
            {
                AddPlayerSayPlugin(message, (PlayerSayPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerSayPlugin(string message, LuaScope script, LuaTable parameters)
        {
            AddPlayerSayPlugin(message, new LuaScriptingPlayerSayPlugin(script, parameters) );
        }

        public PlayerSayPlugin GetPlayerSayPlugin(string message)
        {
            return playerSayPlugins.GetPlugin(message);
        }

        private PluginListCached<PlayerLoginPlugin> playerLoginPlugins = new PluginListCached<PlayerLoginPlugin>();

        public void AddPlayerLoginPlugin(PlayerLoginPlugin playerLoginPlugin)
        {
            playerLoginPlugins.AddPlugin(playerLoginPlugin);
        }

        public void AddPlayerLoginPlugin(string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerLoginPlugin(new LuaScriptingPlayerLoginPlugin(fileName) );
            }
            else
            {
                AddPlayerLoginPlugin( (PlayerLoginPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerLoginPlugin(LuaScope script, LuaTable parameters)
        {
            AddPlayerLoginPlugin(new LuaScriptingPlayerLoginPlugin(script, parameters) );
        }

        public IEnumerable<PlayerLoginPlugin> GetPlayerLoginPlugins()
        {
            return playerLoginPlugins.GetPlugins();
        }

        private PluginListCached<PlayerLogoutPlugin> playerLogoutPlugins = new PluginListCached<PlayerLogoutPlugin>();

        public void AddPlayerLogoutPlugin(PlayerLogoutPlugin playerLogoutPlugin)
        {
            playerLogoutPlugins.AddPlugin(playerLogoutPlugin);
        }

        public void AddPlayerLogoutPlugin(string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddPlayerLogoutPlugin(new LuaScriptingPlayerLogoutPlugin(fileName) );
            }
            else
            {
                AddPlayerLogoutPlugin( (PlayerLogoutPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddPlayerLogoutPlugin(LuaScope script, LuaTable parameters)
        {
            AddPlayerLogoutPlugin(new LuaScriptingPlayerLogoutPlugin(script, parameters) );
        }

        public IEnumerable<PlayerLogoutPlugin> GetPlayerLogoutPlugins()
        {
            return playerLogoutPlugins.GetPlugins();
        }

        private PluginDictionary<string, DialoguePlugin> dialoguePlugins = new PluginDictionary<string, DialoguePlugin>();

        public void AddDialoguePlugin(string name, Func<DialoguePlugin> dialoguePlugin)
        {
            dialoguePlugins.AddPlugin(name, dialoguePlugin);
        }

        public void AddDialoguePlugin(string name, string fileName)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddDialoguePlugin(name, () => new LuaScriptingDialoguePlugin(fileName) );
            }
            else
            {
                AddDialoguePlugin(name, () => (DialoguePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName) ) );
            }
        }

        public void AddDialoguePlugin(string name, LuaScope script, LuaTable parameters)
        {
            AddDialoguePlugin(name, () => new LuaScriptingDialoguePlugin(script, parameters) );
        }

        public DialoguePlugin GetDialoguePlugin(string name)
        {
            return dialoguePlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<string, SpellPlugin> spellPluginsRequiresTarget = new PluginDictionaryCached<string, SpellPlugin>();
        private PluginDictionaryCached<string, SpellPlugin> spellPlugins = new PluginDictionaryCached<string, SpellPlugin>();

        public void AddSpellPlugin(bool requiresTarget, SpellPlugin spellPlugin)
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

        public void AddSpellPlugin(bool requiresTarget, string fileName, Spell spell)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddSpellPlugin(requiresTarget, new LuaScriptingSpellPlugin(fileName, spell) );
            }
            else
            {
                AddSpellPlugin(requiresTarget, (SpellPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), spell) );
            }
        }

        public void AddSpellPlugin(bool requiresTarget, LuaScope script, LuaTable parameters, Spell spell)
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

        public void AddRunePlugin(bool requiresTarget, RunePlugin runePlugin)
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

        public void AddRunePlugin(bool requiresTarget, string fileName, Rune rune)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddRunePlugin(requiresTarget, new LuaScriptingRunePlugin(fileName, rune) );
            }
            else
            {
                AddRunePlugin(requiresTarget, (RunePlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), rune) );
            }
        }

        public void AddRunePlugin(bool requiresTarget, LuaScope script, LuaTable parameters, Rune rune)
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

        public void AddWeaponPlugin(WeaponPlugin weaponPlugin)
        {
            weapons.Add(weaponPlugin.Weapon);

            weaponPlugins.AddPlugin(weaponPlugin.Weapon.OpenTibiaId, weaponPlugin);
        }

        public void AddWeaponPlugin(string fileName, Weapon weapon)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddWeaponPlugin(new LuaScriptingWeaponPlugin(fileName, weapon) );
            }
            else
            {
                AddWeaponPlugin( (WeaponPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), weapon) );
            }
        }

        public void AddWeaponPlugin(LuaScope script, LuaTable parameters, Weapon weapon)
        {
            AddWeaponPlugin(new LuaScriptingWeaponPlugin(script, parameters, weapon) );
        }

        public WeaponPlugin GetWeaponPlugin(ushort openTibiaId)
        {
            return weaponPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, AmmunitionPlugin> ammunitionPlugins = new PluginDictionaryCached<ushort, AmmunitionPlugin>();

        public void AddAmmunitionPlugin(AmmunitionPlugin ammunitionPlugin)
        {
            ammunitions.Add(ammunitionPlugin.Ammunition);

            ammunitionPlugins.AddPlugin(ammunitionPlugin.Ammunition.OpenTibiaId, ammunitionPlugin);
        }

        public void AddAmmunitionPlugin(string fileName, Ammunition ammunition)
        {
            if (fileName.EndsWith(".lua") )
            {
                AddAmmunitionPlugin(new LuaScriptingAmmunitionPlugin(fileName, ammunition) );
            }
            else
            {
                AddAmmunitionPlugin( (AmmunitionPlugin)Activator.CreateInstance(server.PluginLoader.GetType(fileName), ammunition) );
            }
        }

        public void AddAmmunitionPlugin(LuaScope script, LuaTable parameters, Ammunition ammunition)
        {
            AddAmmunitionPlugin(new LuaScriptingAmmunitionPlugin(script, parameters, ammunition) );
        }

        public AmmunitionPlugin GetAmmunitionPlugin(ushort openTibiaId)
        {
            return ammunitionPlugins.GetPlugin(openTibiaId);
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

                dialoguePlugins.GetPlugins(),

                spellPluginsRequiresTarget.GetPlugins(),

                spellPlugins.GetPlugins(),

                runePluginsRequiresTarget.GetPlugins(),

                runePlugins.GetPlugins(),

                weaponPlugins.GetPlugins(),

                ammunitionPlugins.GetPlugins()
            };

            foreach (var pluginList in pluginLists)
            {
                foreach (var plugin in pluginList)
                {
                    plugin.Stop();
                }
            }
        }

        public void Dispose()
        {
            foreach (var autoLoadPlugin in autoLoadPlugins)
            {
                autoLoadPlugin.Dispose();
            }

            script.Dispose();
        }
    }
}