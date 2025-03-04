using NLua;
using OpenTibia.Common.Objects;
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
        private class PluginListCached<TValue, TLuaImplementation> where TValue : Plugin
                                                                   where TLuaImplementation : TValue
        {
            private IServer server;

            public PluginListCached(IServer server)
            {
                this.server = server;
            }

            private List<TValue> plugins = new List<TValue>();

            private void AddPlugin(TValue plugin)
            {
                plugin.Start();

                plugins.Add(plugin);
            }

            public void AddPlugin(string fileName, ILuaScope script, LuaTable parameters, params object[] args)
            {
                if (fileName != null)
                {
                    if (fileName.EndsWith(".lua") )
                    {
                        AddPlugin( (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ fileName, parameters, ..args ] ) );
                    }
                    else
                    {
                        AddPlugin( (TValue)Activator.CreateInstance(server.PluginLoader.GetType(fileName), [ ..args ] ) );
                    }
                }
                else
                {
                    AddPlugin( (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ script, parameters, ..args ] ) );
                }
            }

            public IEnumerable<TValue> GetPlugins()
            {
                return plugins;
            }
        }

        private class PluginDictionaryCached<TKey, TValue, TLuaImplementation> where TValue : Plugin
                                                                               where TLuaImplementation : TValue
        {
            private IServer server;

            public PluginDictionaryCached(IServer server)
            {
                this.server = server;
            }

            private Dictionary<TKey, TValue> plugins = new Dictionary<TKey, TValue>();

            private void AddPlugin(TKey key, TValue plugin)
            {
                plugin.Start();

                plugins.Add(key, plugin);
            }

            public void AddPlugin(TKey key, string fileName, ILuaScope script, LuaTable parameters, params object[] args)
            {
                if (fileName != null)
                {
                    if (fileName.EndsWith(".lua") )
                    {
                        AddPlugin(key, (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ fileName, parameters, ..args ] ) );
                    }
                    else
                    {
                        AddPlugin(key, (TValue)Activator.CreateInstance(server.PluginLoader.GetType(fileName), [ ..args ] ) );
                    }
                }
                else
                {
                    AddPlugin(key, (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ script, parameters, ..args ] ) );
                }
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

        private class PluginDictionary<TKey, TValue, TLuaImplementation> where TValue : Plugin
                                                                         where TLuaImplementation : TValue
        {
            private IServer server;

            public PluginDictionary(IServer server)
            {
                this.server = server;
            }

            private Dictionary<TKey, Func<TValue>> factories = new Dictionary<TKey, Func<TValue>>();

            private List<TValue> plugins = new List<TValue>();

            private void AddPlugin(TKey key, Func<TValue> factory)
            {
                factories.Add(key, factory);
            }

            public void AddPlugin(TKey key, string fileName, ILuaScope script, LuaTable parameters, params object[] args)
            {
                if (fileName != null)
                {
                    if (fileName.EndsWith(".lua") )
                    {
                        AddPlugin(key, () => (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ fileName, parameters, ..args ] ) );
                    }
                    else
                    {
                        AddPlugin(key, () => (TValue)Activator.CreateInstance(server.PluginLoader.GetType(fileName), [ ..args ] ) );
                    }
                }
                else
                {
                    AddPlugin(key, () => (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ script, parameters, ..args ] ) );
                }
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

        private class ItemPluginDictionaryCached<TValue, TLuaImplementation> where TValue : Plugin
                                                                             where TLuaImplementation : TValue
        {
            private IServer server;

            public ItemPluginDictionaryCached(IServer server)
            {
                this.server = server;
            }

            private PluginDictionaryCached<uint, TValue, TLuaImplementation> gameObjects;

            private PluginDictionaryCached<ushort, TValue, TLuaImplementation> uniqueIds;

            private PluginDictionaryCached<ushort, TValue, TLuaImplementation> actionIds;

            private PluginDictionaryCached<ushort, TValue, TLuaImplementation> openTibiaIds;

            public void AddPlugin(uint? id, ushort? uniqueId, ushort? actionId, ushort? openTibiaId, string fileName, ILuaScope script, LuaTable parameters, params object[] args)
            {
                if (id != null)
                {
                    if (gameObjects == null)
                    {
                        gameObjects = new PluginDictionaryCached<uint, TValue, TLuaImplementation>(server);
                    }

                    gameObjects.AddPlugin(id.Value, fileName, script, parameters, args);
                }
                else if (uniqueId != null)
                {
                    if (uniqueIds == null)
                    {
                        uniqueIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(server);
                    }

                    uniqueIds.AddPlugin(uniqueId.Value, fileName, script, parameters, args);
                }
                else if (actionId != null)
                {
                    if (actionIds == null)
                    {
                        actionIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(server);
                    }

                    actionIds.AddPlugin(actionId.Value, fileName, script, parameters, args);
                }
                else if (openTibiaId != null)
                {
                    if (openTibiaIds == null)
                    {
                        openTibiaIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(server);
                    }

                    openTibiaIds.AddPlugin(openTibiaId.Value, fileName, script, parameters, args);
                }
            }

            public TValue GetPlugin(Item item)
            {
                if (gameObjects != null)
                {
                    TValue plugin = gameObjects.GetPlugin(item.Id);

                    if (plugin != null)
                    {
                        return plugin;
                    }
                }

                if (uniqueIds != null)
                {
                    TValue plugin = uniqueIds.GetPlugin(item.UniqueId);

                    if (plugin != null)
                    {
                        return plugin;
                    }
                }

                if (actionIds != null)
                {
                    TValue plugin = actionIds.GetPlugin(item.ActionId);

                    if (plugin != null)
                    {
                        return plugin;
                    }
                }

                if (openTibiaIds != null)
                {
                    TValue plugin = openTibiaIds.GetPlugin(item.Metadata.OpenTibiaId);

                    if (plugin != null)
                    {
                        return plugin;
                    }
                }

                return null;
            }

            public IEnumerable<TValue> GetPlugins()
            {
                if (gameObjects != null)
                {
                    foreach (var plugin in gameObjects.GetPlugins() )
                    {
                        yield return plugin;
                    }
                }

                if (uniqueIds != null)
                {
                    foreach (var plugin in uniqueIds.GetPlugins() )
                    {
                        yield return plugin;
                    }
                }

                if (actionIds != null)
                {
                    foreach (var plugin in actionIds.GetPlugins() )
                    {
                        yield return plugin;
                    }
                }

                if (openTibiaIds != null)
                {
                    foreach (var plugin in openTibiaIds.GetPlugins() )
                    {
                        yield return plugin;
                    }
                }               
            }
        }

        private class AutoLoadPlugin : IDisposable
        {
            private ILuaScope script;

            public AutoLoadPlugin(PluginCollection pluginCollection, string filePath)
            {
                ILuaScope scripts = pluginCollection.server.LuaScripts.LoadLib(
                    pluginCollection.server.PathResolver.GetFullPath("data/plugins/scripts/lib.lua"), 
                    pluginCollection.server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    pluginCollection.server.PathResolver.GetFullPath("data/lib.lua") );
                
                var initializations = new List<(string NodeType, LuaTable Parameters)>();

                scripts["registerplugin"] = (string type, LuaTable parameters) =>
                {
                    initializations.Add( (type, parameters) );
                };

                script = pluginCollection.server.LuaScripts.LoadScript(pluginCollection.server.PathResolver.GetFullPath(filePath), scripts);

                scripts["registerplugin"] = null;

                foreach (var initialization in initializations)
                {
                    if (initialization.NodeType == "actions")
                    {
                        pluginCollection.ParseActions(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "movements")
                    {
                        pluginCollection.ParseMovements(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "talkactions")
                    {
                        pluginCollection.ParseTalkActions(null, script, initialization.Parameters);                        
                    }
                    else if (initialization.NodeType == "creaturescripts")
                    {
                        pluginCollection.ParseCreatureScripts(null, script, initialization.Parameters);              
                    }                    
                    else if (initialization.NodeType == "globalevents")
                    {
                        pluginCollection.ParseCreatureGlobalEvents(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "items")
                    {
                        pluginCollection.ParseItems(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "monsters")
                    {
                        pluginCollection.ParseMonsters(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "npcs")
                    {
                        pluginCollection.ParseNpcs(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "players")
                    {
                        pluginCollection.ParsePlayers(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "spells")
                    {
                        pluginCollection.ParseSpells(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "runes")
                    {
                        pluginCollection.ParseRunes(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "weapons")
                    {
                        pluginCollection.ParseWeapons(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "ammunitions")
                    {
                        pluginCollection.ParseAmmunitions(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "raids")
                    {
                        pluginCollection.ParseRaids(null, script, initialization.Parameters);
                    }
                    else if (initialization.NodeType == "monsterattacks")
                    {
                        pluginCollection.ParseMonsterAttacks(null, script, initialization.Parameters);
                    }
                }
            }

            ~AutoLoadPlugin()
            {
                Dispose(false);
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

            this.autoLoadPlugins = new List<AutoLoadPlugin>();

            this.playerRotateItemPlugins = new ItemPluginDictionaryCached<PlayerRotateItemPlugin, LuaScriptingPlayerRotateItemPlugin>(server);

            this.playerUseItemPlugins = new ItemPluginDictionaryCached<PlayerUseItemPlugin, LuaScriptingPlayerUseItemPlugin>(server);

            this.playerUseItemWithItemPluginsAllowFarUse = new ItemPluginDictionaryCached<PlayerUseItemWithItemPlugin, LuaScriptingPlayerUseItemWithItemPlugin>(server); this.playerUseItemWithItemPlugins = new ItemPluginDictionaryCached<PlayerUseItemWithItemPlugin, LuaScriptingPlayerUseItemWithItemPlugin>(server);

            this.playerUseItemWithCreaturePluginsAllowFarUse = new ItemPluginDictionaryCached<PlayerUseItemWithCreaturePlugin, LuaScriptingPlayerUseItemWithCreaturePlugin>(server); this.playerUseItemWithCreaturePlugins = new ItemPluginDictionaryCached<PlayerUseItemWithCreaturePlugin, LuaScriptingPlayerUseItemWithCreaturePlugin>(server);
    
            this.playerMoveCreaturePlugins = new PluginDictionaryCached<string, PlayerMoveCreaturePlugin, LuaScriptingPlayerMoveCreaturePlugin>(server);
     
            this.playerMoveItemPlugins = new ItemPluginDictionaryCached<PlayerMoveItemPlugin, LuaScriptingPlayerMoveItemPlugin>(server);
   
            this.creatureStepInPlugins = new ItemPluginDictionaryCached<CreatureStepInPlugin, LuaScriptingCreatureStepInPlugin>(server);
 
            this.creatureStepOutPlugins = new ItemPluginDictionaryCached<CreatureStepOutPlugin, LuaScriptingCreatureStepOutPlugin>(server);
   
            this.inventoryEquipPlugins = new ItemPluginDictionaryCached<InventoryEquipPlugin, LuaScriptingInventoryEquipPlugin>(server);
       
            this.inventoryDeEquipPlugins = new ItemPluginDictionaryCached<InventoryDeEquipPlugin, LuaScriptingInventoryDeEquipPlugin>(server);
           
            this.playerSayPlugins = new PluginDictionaryCached<string, PlayerSayPlugin, LuaScriptingPlayerSayPlugin>(server);
   
            this.playerLoginPlugins = new PluginListCached<PlayerLoginPlugin, LuaScriptingPlayerLoginPlugin>(server);
    
            this.playerLogoutPlugins = new PluginListCached<PlayerLogoutPlugin, LuaScriptingPlayerLogoutPlugin>(server);
   
            this.playerAdvanceLevelPlugins = new PluginListCached<PlayerAdvanceLevelPlugin, LuaScriptingPlayerAdvanceLevelPlugin>(server);
     
            this.playerAdvanceSkillPlugins = new PluginListCached<PlayerAdvanceSkillPlugin, LuaScriptingPlayerAdvanceSkillPlugin>(server);
      
            this.creatureDeathPlugins = new PluginListCached<CreatureDeathPlugin, LuaScriptingCreatureDeathPlugin>(server);

            this.creatureKillPlugins = new PluginListCached<CreatureKillPlugin, LuaScriptingCreatureKillPlugin>(server);
      
            this.playerEarnAchievementPlugins = new PluginListCached<PlayerEarnAchievementPlugin, LuaScriptingPlayerEarnAchievementPlugin>(server);
  
            this.serverStartupPlugins = new PluginListCached<ServerStartupPlugin, LuaScriptingServerStartupPlugin>(server);
  
            this.serverShutdownPlugins = new PluginListCached<ServerShutdownPlugin, LuaScriptingServerShutdownPlugin>(server);
            
            this.serverSavePlugins = new PluginListCached<ServerSavePlugin, LuaScriptingServerSavePlugin>(server);
     
            this.serverRecordPlugins = new PluginListCached<ServerRecordPlugin, LuaScriptingServerRecordPlugin>(server);
     
            this.dialoguePlugins = new PluginDictionary<string, DialoguePlugin, LuaScriptingDialoguePlugin>(server);
     
            this.itemCreationPlugins = new PluginDictionaryCached<ushort, ItemCreationPlugin, LuaScriptingItemCreationPlugin>(server);
    
            this.monsterCreationPlugins = new PluginDictionaryCached<string, MonsterCreationPlugin, LuaScriptingMonsterCreationPlugin>(server);
     
            this.npcCreationPlugins = new PluginDictionaryCached<string, NpcCreationPlugin, LuaScriptingNpcCreationPlugin>(server);
   
            this.playerCreationPlugins = new PluginDictionaryCached<string, PlayerCreationPlugin, LuaScriptingPlayerCreationPlugin>(server);
   
            this.spellPluginsRequiresTarget = new PluginDictionaryCached<string, SpellPlugin, LuaScriptingSpellPlugin>(server); this.spellPlugins = new PluginDictionaryCached<string, SpellPlugin, LuaScriptingSpellPlugin>(server);

            this.runePluginsRequiresTarget = new PluginDictionaryCached<ushort, RunePlugin, LuaScriptingRunePlugin>(server); this.runePlugins = new PluginDictionaryCached<ushort, RunePlugin, LuaScriptingRunePlugin>(server);

            this.weaponPlugins = new PluginDictionaryCached<ushort, WeaponPlugin, LuaScriptingWeaponPlugin>(server);

            this.ammunitionPlugins = new PluginDictionaryCached<ushort, AmmunitionPlugin, LuaScriptingAmmunitionPlugin>(server);

            this.raidPlugins = new PluginDictionaryCached<string, RaidPlugin, LuaScriptingRaidPlugin>(server);

            this.monsterAttackPlugins = new PluginDictionaryCached<string, MonsterAttackPlugin, LuaScriptingMonsterAttackPlugin>(server);
        }

        ~PluginCollection()
        {
            Dispose(false);
        }

        private ILuaScope script;

        private List<AutoLoadPlugin> autoLoadPlugins;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/plugins/config.lua"), 
                server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                server.PathResolver.GetFullPath("data/lib.lua") );

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.actions"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseActions(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.movements"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseMovements(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.talkactions"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseTalkActions(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.creaturescripts"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseCreatureScripts(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.globalevents"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseCreatureGlobalEvents(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.items"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseItems(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.monsters"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseMonsters(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.npcs"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseNpcs(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.players"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParsePlayers(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.spells"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseSpells(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.runes"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseRunes(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.weapons"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseWeapons(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.ammunitions"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseAmmunitions(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.raids"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseRaids(fileName, null, parameters);
            }

            foreach (LuaTable parameters in ( (LuaTable)script["plugins.monsterattacks"] ).Values)
            {
                string fileName = LuaScope.GetString(parameters["filename"] );

                ParseMonsterAttacks(fileName, null, parameters);
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

        public void ParseActions(string fileName, ILuaScope script, LuaTable parameters)
        {
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "PlayerRotateItem":
                {
                    playerRotateItemPlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                }
                break;

                case "PlayerUseItem":
                {
                    playerUseItemPlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"]), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                }
                break;

                case "PlayerUseItemWithItem":
                {
                    bool allowFarUse = LuaScope.GetBoolean(parameters["allowfaruse"] );

                    if (allowFarUse)
                    {
                        playerUseItemWithItemPluginsAllowFarUse.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                    }
                    else
                    {
                        playerUseItemWithItemPlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                    }
                }
                break;

                case "PlayerUseItemWithCreature":
                {
                    bool allowFarUse = LuaScope.GetBoolean(parameters["allowfaruse"] );

                    if (allowFarUse)
                    {
                        playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                    }
                    else
                    {
                        playerUseItemWithCreaturePlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                    }
                }
                break;

                case "PlayerMoveItem":
                {
                    playerMoveItemPlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                }
                break;

                case "PlayerMoveCreature":
                {
                    string name = LuaScope.GetString(parameters["name"] );

                    playerMoveCreaturePlugins.AddPlugin(name, fileName, script, parameters);
                }
                break;
            }
        }

        public void ParseMovements(string fileName, ILuaScope script, LuaTable parameters)
        {
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "CreatureStepIn":
                {
                    creatureStepInPlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                }
                break;

                case "CreatureStepOut":
                {
                    creatureStepOutPlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                }
                break;

                case "InventoryEquip":
                {
                    inventoryEquipPlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                }
                break;

                case "InventoryDeEquip":
                {
                    inventoryDeEquipPlugins.AddPlugin(LuaScope.GetNullableUInt32(parameters["id"] ), LuaScope.GetNullableUInt16(parameters["uniqueid"] ), LuaScope.GetNullableUInt16(parameters["actionid"] ), LuaScope.GetNullableUInt16(parameters["opentibiaid"] ), fileName, script, parameters);
                }
                break;
            }
        }

        public void ParseTalkActions(string fileName, ILuaScope script, LuaTable parameters) 
        { 
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "PlayerSay":
                {
                    string message = LuaScope.GetString(parameters["message"] );

                    playerSayPlugins.AddPlugin(message, fileName, script, parameters);
                }
                break;
            }
        }

        public void ParseCreatureScripts(string fileName, ILuaScope script, LuaTable parameters)
        {
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "PlayerLogin":
                {
                    playerLoginPlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "PlayerLogout":
                {
                    playerLogoutPlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "PlayerAdvanceLevel":
                {
                    playerAdvanceLevelPlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "PlayerAdvanceSkill":
                {
                    playerAdvanceSkillPlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "CreatureDeath":
                {
                    creatureDeathPlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "CreatureKill":
                {
                    creatureKillPlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "PlayerEarnAchievement":
                {
                    playerEarnAchievementPlugins.AddPlugin(fileName, script, parameters);
                }
                break;
            }
        }

        public void ParseCreatureGlobalEvents(string fileName, ILuaScope script, LuaTable parameters)
        {
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "ServerStartup":
                {
                    serverStartupPlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "ServerShutdown":
                {
                    serverShutdownPlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "ServerSave":
                {
                    serverSavePlugins.AddPlugin(fileName, script, parameters);
                }
                break;

                case "ServerRecord":
                {
                    serverRecordPlugins.AddPlugin(fileName, script, parameters);
                }
                break;
            }
        }

        public void ParseItems(string fileName, ILuaScope script, LuaTable parameters)
        {
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "ItemCreation":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(parameters["openTibiaId"] );

                        itemCreationPlugins.AddPlugin(openTibiaId, fileName, script, parameters);
                    }
                    break;
            }
        }

        public void ParseMonsters(string fileName, ILuaScope script, LuaTable parameters)
        {
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "MonsterCreation":
                    {
                        string name = LuaScope.GetString(parameters["name"] );

                        monsterCreationPlugins.AddPlugin(name, fileName, script, parameters);
                    }
                    break;
            }
        }

        public void ParseNpcs(string fileName, ILuaScope script, LuaTable parameters)
        {
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "NpcCreation":
                {
                    string name = LuaScope.GetString(parameters["name"] );

                    npcCreationPlugins.AddPlugin(name, fileName, script, parameters);
                }
                break;

                case "Dialogue":
                {
                    string name = LuaScope.GetString(parameters["name"] );

                    dialoguePlugins.AddPlugin(name, fileName, script, parameters);
                }
                break;
            }
        }

        public void ParsePlayers(string fileName, ILuaScope script, LuaTable parameters)
        {
            string type = LuaScope.GetString(parameters["type"] );

            switch (type)
            {
                case "PlayerCreation":
                {
                    string name = LuaScope.GetString(parameters["name"] );

                    playerCreationPlugins.AddPlugin(name, fileName, script, parameters);
                }
                break;
            }
        }

        public void ParseSpells(string fileName, ILuaScope script, LuaTable parameters)
        {
            bool requiresTarget = LuaScope.GetBoolean(parameters["requirestarget"] );

            Spell spell = new Spell()
            {
                Words = LuaScope.GetString(parameters["words"] ),

                Name = LuaScope.GetString(parameters["name"] ),

                Group = LuaScope.GetString(parameters["group"] ),

                Cooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(parameters["cooldown"] ) ),

                GroupCooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(parameters["groupcooldown"] ) ),

                Level = LuaScope.GetInt32(parameters["level"] ),

                Mana = LuaScope.GetInt32(parameters["mana"] ),

                Soul = LuaScope.GetInt32(parameters["soul"] ),

                ConjureOpenTibiaId = LuaScope.GetNullableUInt16(parameters["conjureopentibiaid"] ),

                ConjureCount = LuaScope.GetNullableInt32(parameters["conjurecount"] ),

                Premium = LuaScope.GetBoolean(parameters["premium"] ),

                Vocations = ( (LuaTable)parameters["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
            };

            spells.Add(spell);

            if (requiresTarget)
            {
                spellPluginsRequiresTarget.AddPlugin(spell.Words, fileName, script, parameters, spell);
            }
            else
            {
                spellPlugins.AddPlugin(spell.Words, fileName, script, parameters, spell);
            }
        }

        public void ParseRunes(string fileName, ILuaScope script, LuaTable parameters)
        {
            bool requiresTarget = LuaScope.GetBoolean(parameters["requirestarget"] );

            Rune rune = new Rune()
            {
                OpenTibiaId = LuaScope.GetUInt16(parameters["opentibiaid"] ),

                Name = LuaScope.GetString(parameters["name"] ),

                Group = LuaScope.GetString(parameters["group"] ),

                GroupCooldown = TimeSpan.FromSeconds(LuaScope.GetInt32(parameters["groupcooldown"] ) ),

                Level = LuaScope.GetInt32(parameters["level"] ),

                MagicLevel = LuaScope.GetInt32(parameters["magiclevel"] ),

                Vocations = ( (LuaTable)parameters["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
            };

            runes.Add(rune);

            if (requiresTarget)
            {
                runePluginsRequiresTarget.AddPlugin(rune.OpenTibiaId, fileName, script, parameters, rune);
            }
            else
            {
                runePlugins.AddPlugin(rune.OpenTibiaId, fileName, script, parameters, rune);
            }
        }

        public void ParseWeapons(string fileName, ILuaScope script, LuaTable parameters)
        {
            Weapon weapon = new Weapon()
            {
                OpenTibiaId = LuaScope.GetUInt16(parameters["opentibiaid"] ),

                Level = LuaScope.GetInt32(parameters["level"] ),

                Mana = LuaScope.GetInt32(parameters["mana"] ),

                Vocations = ( (LuaTable)parameters["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
            };

            weapons.Add(weapon);

            weaponPlugins.AddPlugin(weapon.OpenTibiaId, fileName, script, parameters, weapon);
        }

        public void ParseAmmunitions(string fileName, ILuaScope script, LuaTable parameters)
        {
            Ammunition ammunition = new Ammunition()
            {
                OpenTibiaId = LuaScope.GetUInt16(parameters["opentibiaid"] ),

                Level = LuaScope.GetInt32(parameters["level"] )
            };

            ammunitions.Add(ammunition);

            ammunitionPlugins.AddPlugin(ammunition.OpenTibiaId, fileName, script, parameters, ammunition);
        }

        public void ParseRaids(string fileName, ILuaScope script, LuaTable parameters)
        {
            Raid raid = new Raid()
            {
                Name = LuaScope.GetString(parameters["name"] ),

                Repeatable = LuaScope.GetBoolean(parameters["repeatable"] ),

                Interval = LuaScope.GetInt32(parameters["interval"] ),

                Chance = LuaScope.GetDouble(parameters["chance"] ),

                Enabled = LuaScope.GetBoolean(parameters["enabled"] )
            };

            raids.Add(raid);

            raidPlugins.AddPlugin(raid.Name, fileName, script, parameters, raid);
        }

        public void ParseMonsterAttacks(string fileName, ILuaScope script, LuaTable parameters)
        {
            string name = LuaScope.GetString(parameters["name"] );

            monsterAttackPlugins.AddPlugin(name, fileName, script, parameters);
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

        private ItemPluginDictionaryCached<PlayerRotateItemPlugin, LuaScriptingPlayerRotateItemPlugin> playerRotateItemPlugins;

        public PlayerRotateItemPlugin GetPlayerRotateItemPlugin(Item item)
        {
            return playerRotateItemPlugins.GetPlugin(item);
        }

        private ItemPluginDictionaryCached<PlayerUseItemPlugin, LuaScriptingPlayerUseItemPlugin> playerUseItemPlugins;

        public PlayerUseItemPlugin GetPlayerUseItemPlugin(Item item)
        {
            return playerUseItemPlugins.GetPlugin(item);
        }

        private ItemPluginDictionaryCached<PlayerUseItemWithItemPlugin, LuaScriptingPlayerUseItemWithItemPlugin> playerUseItemWithItemPluginsAllowFarUse;
        private ItemPluginDictionaryCached<PlayerUseItemWithItemPlugin, LuaScriptingPlayerUseItemWithItemPlugin> playerUseItemWithItemPlugins;

        public PlayerUseItemWithItemPlugin GetPlayerUseItemWithItemPlugin(bool allowFarUse, Item item)
        {
            if (allowFarUse)
            {
                return playerUseItemWithItemPluginsAllowFarUse.GetPlugin(item);
            }
            else
            {
                return playerUseItemWithItemPlugins.GetPlugin(item);
            }
        }

        private ItemPluginDictionaryCached<PlayerUseItemWithCreaturePlugin, LuaScriptingPlayerUseItemWithCreaturePlugin> playerUseItemWithCreaturePluginsAllowFarUse;
        private ItemPluginDictionaryCached<PlayerUseItemWithCreaturePlugin, LuaScriptingPlayerUseItemWithCreaturePlugin> playerUseItemWithCreaturePlugins;

        public PlayerUseItemWithCreaturePlugin GetPlayerUseItemWithCreaturePlugin(bool allowFarUse, Item item)
        {
            if (allowFarUse)
            {
                return playerUseItemWithCreaturePluginsAllowFarUse.GetPlugin(item);
            }
            else
            {
                return playerUseItemWithCreaturePlugins.GetPlugin(item);
            }
        }

        private PluginDictionaryCached<string, PlayerMoveCreaturePlugin, LuaScriptingPlayerMoveCreaturePlugin> playerMoveCreaturePlugins;

        public PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(string name)
        {
            return playerMoveCreaturePlugins.GetPlugin(name);
        }

        private ItemPluginDictionaryCached<PlayerMoveItemPlugin, LuaScriptingPlayerMoveItemPlugin> playerMoveItemPlugins;

        public PlayerMoveItemPlugin GetPlayerMoveItemPlugin(Item item)
        {
            return playerMoveItemPlugins.GetPlugin(item);
        }

        private ItemPluginDictionaryCached<CreatureStepInPlugin, LuaScriptingCreatureStepInPlugin> creatureStepInPlugins;

        public CreatureStepInPlugin GetCreatureStepInPlugin(Item item)
        {
            return creatureStepInPlugins.GetPlugin(item);
        }

        private ItemPluginDictionaryCached<CreatureStepOutPlugin, LuaScriptingCreatureStepOutPlugin> creatureStepOutPlugins;

        public CreatureStepOutPlugin GetCreatureStepOutPlugin(Item item)
        {
            return creatureStepOutPlugins.GetPlugin(item);
        }

        private ItemPluginDictionaryCached<InventoryEquipPlugin, LuaScriptingInventoryEquipPlugin> inventoryEquipPlugins;

        public InventoryEquipPlugin GetInventoryEquipPlugin(Item item)
        {
            return inventoryEquipPlugins.GetPlugin(item);
        }

        private ItemPluginDictionaryCached<InventoryDeEquipPlugin, LuaScriptingInventoryDeEquipPlugin> inventoryDeEquipPlugins;

        public InventoryDeEquipPlugin GetInventoryDeEquipPlugin(Item item)
        {
            return inventoryDeEquipPlugins.GetPlugin(item);
        }

        private PluginDictionaryCached<string, PlayerSayPlugin, LuaScriptingPlayerSayPlugin> playerSayPlugins;

        public PlayerSayPlugin GetPlayerSayPlugin(string message)
        {
            return playerSayPlugins.GetPlugin(message);
        }

        private PluginListCached<PlayerLoginPlugin, LuaScriptingPlayerLoginPlugin> playerLoginPlugins;

        public IEnumerable<PlayerLoginPlugin> GetPlayerLoginPlugins()
        {
            return playerLoginPlugins.GetPlugins();
        }

        private PluginListCached<PlayerLogoutPlugin, LuaScriptingPlayerLogoutPlugin> playerLogoutPlugins;

        public IEnumerable<PlayerLogoutPlugin> GetPlayerLogoutPlugins()
        {
            return playerLogoutPlugins.GetPlugins();
        }

        private PluginListCached<PlayerAdvanceLevelPlugin, LuaScriptingPlayerAdvanceLevelPlugin> playerAdvanceLevelPlugins;

        public IEnumerable<PlayerAdvanceLevelPlugin> GetPlayerAdvanceLevelPlugins()
        {
            return playerAdvanceLevelPlugins.GetPlugins();
        }

        private PluginListCached<PlayerAdvanceSkillPlugin, LuaScriptingPlayerAdvanceSkillPlugin> playerAdvanceSkillPlugins;

        public IEnumerable<PlayerAdvanceSkillPlugin> GetPlayerAdvanceSkillPlugins()
        {
            return playerAdvanceSkillPlugins.GetPlugins();
        }

        private PluginListCached<CreatureDeathPlugin, LuaScriptingCreatureDeathPlugin> creatureDeathPlugins;

        public IEnumerable<CreatureDeathPlugin> GetCreatureDeathPlugins()
        {
            return creatureDeathPlugins.GetPlugins();
        }

        private PluginListCached<CreatureKillPlugin, LuaScriptingCreatureKillPlugin> creatureKillPlugins;

        public IEnumerable<CreatureKillPlugin> GetCreatureKillPlugins()
        {
            return creatureKillPlugins.GetPlugins();
        }

        private PluginListCached<PlayerEarnAchievementPlugin, LuaScriptingPlayerEarnAchievementPlugin> playerEarnAchievementPlugins;

        public IEnumerable<PlayerEarnAchievementPlugin> GetPlayerEarnAchievementPlugins()
        {
            return playerEarnAchievementPlugins.GetPlugins();
        }

        private PluginListCached<ServerStartupPlugin, LuaScriptingServerStartupPlugin> serverStartupPlugins;

        public IEnumerable<ServerStartupPlugin> GetServerStartupPlugins()
        {
            return serverStartupPlugins.GetPlugins();
        }

        private PluginListCached<ServerShutdownPlugin, LuaScriptingServerShutdownPlugin> serverShutdownPlugins;

        public IEnumerable<ServerShutdownPlugin> GetServerShutdownPlugins()
        {
            return serverShutdownPlugins.GetPlugins();
        }

        private PluginListCached<ServerSavePlugin, LuaScriptingServerSavePlugin> serverSavePlugins;

        public IEnumerable<ServerSavePlugin> GetServerSavePlugins()
        {
            return serverSavePlugins.GetPlugins();
        }

        private PluginListCached<ServerRecordPlugin, LuaScriptingServerRecordPlugin> serverRecordPlugins;

        public IEnumerable<ServerRecordPlugin> GetServerRecordPlugins()
        {
            return serverRecordPlugins.GetPlugins();
        }

        private PluginDictionary<string, DialoguePlugin, LuaScriptingDialoguePlugin> dialoguePlugins;

        public DialoguePlugin GetDialoguePlugin(string name)
        {
            return dialoguePlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<ushort, ItemCreationPlugin, LuaScriptingItemCreationPlugin> itemCreationPlugins;

        public ItemCreationPlugin GetItemCreationPlugin(ushort openTibiaId)
        {
            return itemCreationPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<string, MonsterCreationPlugin, LuaScriptingMonsterCreationPlugin> monsterCreationPlugins;

        public MonsterCreationPlugin GetMonsterCreationPlugin(string name)
        {
            return monsterCreationPlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<string, NpcCreationPlugin, LuaScriptingNpcCreationPlugin> npcCreationPlugins;

        public NpcCreationPlugin GetNpcCreationPlugin(string name)
        {
            return npcCreationPlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<string, PlayerCreationPlugin, LuaScriptingPlayerCreationPlugin> playerCreationPlugins;

        public PlayerCreationPlugin GetPlayerCreationPlugin(string name)
        {
            return playerCreationPlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<string, SpellPlugin, LuaScriptingSpellPlugin> spellPluginsRequiresTarget;
        private PluginDictionaryCached<string, SpellPlugin, LuaScriptingSpellPlugin> spellPlugins;

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

        private PluginDictionaryCached<ushort, RunePlugin, LuaScriptingRunePlugin> runePluginsRequiresTarget;
        private PluginDictionaryCached<ushort, RunePlugin, LuaScriptingRunePlugin> runePlugins;

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

        private PluginDictionaryCached<ushort, WeaponPlugin, LuaScriptingWeaponPlugin> weaponPlugins;

        public WeaponPlugin GetWeaponPlugin(ushort openTibiaId)
        {
            return weaponPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, AmmunitionPlugin, LuaScriptingAmmunitionPlugin> ammunitionPlugins;

        public AmmunitionPlugin GetAmmunitionPlugin(ushort openTibiaId)
        {
            return ammunitionPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<string, RaidPlugin, LuaScriptingRaidPlugin> raidPlugins;

        public RaidPlugin GetRaidPlugin(string name)
        {
            return raidPlugins.GetPlugin(name);
        }

        private PluginDictionaryCached<string, MonsterAttackPlugin, LuaScriptingMonsterAttackPlugin> monsterAttackPlugins;

        public MonsterAttackPlugin GetMonsterAttackPlugin(string name)
        {
            return monsterAttackPlugins.GetPlugin(name);
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

                creatureKillPlugins.GetPlugins(),

                playerEarnAchievementPlugins.GetPlugins(),

                serverStartupPlugins.GetPlugins(),

                serverSavePlugins.GetPlugins(),

                serverRecordPlugins.GetPlugins(),

                serverShutdownPlugins.GetPlugins(),

                dialoguePlugins.GetPlugins(),

                itemCreationPlugins.GetPlugins(),

                monsterCreationPlugins.GetPlugins(),

                npcCreationPlugins.GetPlugins(),

                playerCreationPlugins.GetPlugins(),

                spellPluginsRequiresTarget.GetPlugins(),

                spellPlugins.GetPlugins(),

                runePluginsRequiresTarget.GetPlugins(),

                runePlugins.GetPlugins(),

                weaponPlugins.GetPlugins(),

                ammunitionPlugins.GetPlugins(),

                raidPlugins.GetPlugins(),

                monsterAttackPlugins.GetPlugins()
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