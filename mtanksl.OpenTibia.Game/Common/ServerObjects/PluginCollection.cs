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
            private PluginCollection pluginCollection;

            public PluginListCached(PluginCollection pluginCollection)
            {
                this.pluginCollection = pluginCollection;
            }

            private List<TValue> plugins = new List<TValue>();

            public void AddPlugin(TValue plugin)
            {
                plugin.Start();

                plugins.Add(plugin);
            }

            public void AddPlugin(string fileName, LuaTable parameters, params object[] args)
            {
                if (fileName.EndsWith(".lua") )
                {
                    AddPlugin( (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ fileName, parameters, ..args ] ) );
                }
                else
                {
                    AddPlugin( (TValue)Activator.CreateInstance(pluginCollection.server.PluginLoader.GetType(fileName), [ ..args ] ) );
                }
            }

            public void AddPlugin(ILuaScope script, LuaTable parameters, params object[] args)
            {                   
                AddPlugin( (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ script, parameters, ..args ] ) );
            }

            public IEnumerable<TValue> GetPlugins()
            {
                return plugins;
            }
        }

        private class PluginDictionaryCached<TKey, TValue, TLuaImplementation> where TValue : Plugin
                                                                               where TLuaImplementation : TValue
        {
            private PluginCollection pluginCollection;

            public PluginDictionaryCached(PluginCollection pluginCollection)
            {
                this.pluginCollection = pluginCollection;
            }

            private Dictionary<TKey, TValue> plugins = new Dictionary<TKey, TValue>();

            public void AddPlugin(TKey key, TValue plugin)
            {
                plugin.Start();

                plugins.Add(key, plugin);
            }

            public void AddPlugin(TKey key, string fileName, LuaTable parameters, params object[] args)
            {
                if (fileName.EndsWith(".lua") )
                {
                    AddPlugin(key, (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ fileName, parameters, ..args ] ) );
                }
                else
                {
                    AddPlugin(key, (TValue)Activator.CreateInstance(pluginCollection.server.PluginLoader.GetType(fileName), [ ..args ] ) );
                }
            }

            public void AddPlugin(TKey key, ILuaScope script, LuaTable parameters, params object[] args)
            {                   
                AddPlugin(key, (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ script, parameters, ..args ] ) );
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
            private PluginCollection pluginCollection;

            public PluginDictionary(PluginCollection pluginCollection)
            {
                this.pluginCollection = pluginCollection;
            }

            private Dictionary<TKey, Func<TValue>> factories = new Dictionary<TKey, Func<TValue>>();

            private List<TValue> plugins = new List<TValue>();

            public void AddPlugin(TKey key, Func<TValue> factory)
            {
                factories.Add(key, factory);
            }

            public void AddPlugin(TKey key, string fileName, LuaTable parameters, params object[] args)
            {
                if (fileName.EndsWith(".lua") )
                {
                    AddPlugin(key, () => (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ fileName, parameters, ..args ] ) );
                }
                else
                {
                    AddPlugin(key, () => (TValue)Activator.CreateInstance(pluginCollection.server.PluginLoader.GetType(fileName), [ ..args ] ) );
                }
            }

            public void AddPlugin(TKey key, ILuaScope script, LuaTable parameters, params object[] args)
            {                   
                AddPlugin(key, () => (TValue)Activator.CreateInstance(typeof(TLuaImplementation), [ script, parameters, ..args ] ) );
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
            private PluginCollection pluginCollection;

            public ItemPluginDictionaryCached(PluginCollection pluginCollection)
            {
                this.pluginCollection = pluginCollection;
            }

            private PluginDictionaryCached<Item, TValue, TLuaImplementation> items;

            public void ItemAddPlugin(Item item, TValue plugin)
            {
                if (items == null)
                {
                    items = new PluginDictionaryCached<Item, TValue, TLuaImplementation>(pluginCollection);
                }

                items.AddPlugin(item, plugin);
            }

            public void ItemAddPlugin(Item item, string fileName, LuaTable parameters, params object[] args)
            {
                if (items == null)
                {
                    items = new PluginDictionaryCached<Item, TValue, TLuaImplementation>(pluginCollection);
                }

                items.AddPlugin(item, fileName, parameters, args);
            }

            public void ItemAddPlugin(Item item, ILuaScope script, LuaTable parameters, params object[] args)
            {
                if (items == null)
                {
                    items = new PluginDictionaryCached<Item, TValue, TLuaImplementation>(pluginCollection);
                }

                items.AddPlugin(item, script, parameters, args);
            }

            private PluginDictionaryCached<ushort, TValue, TLuaImplementation> uniqueIds;

            public void UniqueIdAddPlugin(ushort uniqueId, TValue plugin)
            {
                if (uniqueIds == null)
                {
                    uniqueIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                uniqueIds.AddPlugin(uniqueId, plugin);
            }

            public void UniqueIdAddPlugin(ushort uniqueId, string fileName, LuaTable parameters, params object[] args)
            {
                if (uniqueIds == null)
                {
                    uniqueIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                uniqueIds.AddPlugin(uniqueId, fileName, parameters, args);
            }

            public void UniqueIdAddPlugin(ushort uniqueId, ILuaScope script, LuaTable parameters, params object[] args)
            {
                if (uniqueIds == null)
                {
                    uniqueIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                uniqueIds.AddPlugin(uniqueId, script, parameters, args);
            }

            private PluginDictionaryCached<ushort, TValue, TLuaImplementation> actionIds;

            public void ActionIdAddPlugin(ushort actionId, TValue plugin)
            {
                if (actionIds == null)
                {
                    actionIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                actionIds.AddPlugin(actionId, plugin);
            }

            public void ActionIdAddPlugin(ushort actionId, string fileName, LuaTable parameters, params object[] args)
            {
                if (actionIds == null)
                {
                    actionIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                actionIds.AddPlugin(actionId, fileName, parameters, args);
            }

            public void ActionIdAddPlugin(ushort actionId, ILuaScope script, LuaTable parameters, params object[] args)
            {
                if (actionIds == null)
                {
                    actionIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                actionIds.AddPlugin(actionId, script, parameters, args);
            }

            private PluginDictionaryCached<ushort, TValue, TLuaImplementation> openTibiaIds;

            public void OpenTibiaIdAddPlugin(ushort openTibiaId, TValue plugin)
            {
                if (openTibiaIds == null)
                {
                    openTibiaIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                openTibiaIds.AddPlugin(openTibiaId, plugin);
            }

            public void OpenTibiaIdAddPlugin(ushort openTibiaId, string fileName, LuaTable parameters, params object[] args)
            {
                if (openTibiaIds == null)
                {
                    openTibiaIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                openTibiaIds.AddPlugin(openTibiaId, fileName, parameters, args);
            }

            public void OpenTibiaIdAddPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters, params object[] args)
            {
                if (openTibiaIds == null)
                {
                    openTibiaIds = new PluginDictionaryCached<ushort, TValue, TLuaImplementation>(pluginCollection);
                }

                openTibiaIds.AddPlugin(openTibiaId, script, parameters, args);
            }

            public TValue GetPlugin(Item item)
            {
                if (items != null)
                {
                    TValue plugin = items.GetPlugin(item);

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
                if (items != null)
                {
                    foreach (var plugin in items.GetPlugins() )
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
                                if (initialization.Parameters["uniqueid"] != null)
                                {
                                    ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                    pluginCollection.playerRotateItemPlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["actionid"] != null)
                                {
                                    ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                    pluginCollection.playerRotateItemPlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["opentibiaid"] != null)
                                {
                                    ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                    pluginCollection.playerRotateItemPlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                }
                            }
                            break;

                            case "PlayerUseItem":
                            {
                                if (initialization.Parameters["uniqueid"] != null)
                                {
                                    ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                    pluginCollection.playerUseItemPlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["actionid"] != null)
                                {
                                    ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                    pluginCollection.playerUseItemPlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["opentibiaid"] != null)
                                {
                                    ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                    pluginCollection.playerUseItemPlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                }
                            }
                            break;

                            case "PlayerUseItemWithItem":
                            {
                                bool allowFarUse = LuaScope.GetBoolean(initialization.Parameters["allowfaruse"] );

                                if (allowFarUse)
                                {
                                    if (initialization.Parameters["uniqueid"] != null)
                                    {
                                        ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                        pluginCollection.playerUseItemWithItemPluginsAllowFarUse.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                    }
                                    else if (initialization.Parameters["actionid"] != null)
                                    {
                                        ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                        pluginCollection.playerUseItemWithItemPluginsAllowFarUse.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                    }
                                    else if (initialization.Parameters["opentibiaid"] != null)
                                    {
                                        ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                        pluginCollection.playerUseItemWithItemPluginsAllowFarUse.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                    }               
                                }
                                else
                                {
                                    if (initialization.Parameters["uniqueid"] != null)
                                    {
                                        ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                        pluginCollection.playerUseItemWithItemPlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                    }
                                    else if (initialization.Parameters["actionid"] != null)
                                    {
                                        ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                        pluginCollection.playerUseItemWithItemPlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                    }
                                    else if (initialization.Parameters["opentibiaid"] != null)
                                    {
                                        ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                        pluginCollection.playerUseItemWithItemPlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                    }               
                                }
                            }
                            break;

                            case "PlayerUseItemWithCreature":
                            {
                                bool allowFarUse = LuaScope.GetBoolean(initialization.Parameters["allowfaruse"] );

                                if (allowFarUse)
                                {
                                    if (initialization.Parameters["uniqueid"] != null)
                                    {
                                        ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                        pluginCollection.playerUseItemWithCreaturePluginsAllowFarUse.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                    }
                                    else if (initialization.Parameters["actionid"] != null)
                                    {
                                        ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                        pluginCollection.playerUseItemWithCreaturePluginsAllowFarUse.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                    }
                                    else if (initialization.Parameters["opentibiaid"] != null)
                                    {
                                        ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                        pluginCollection.playerUseItemWithCreaturePluginsAllowFarUse.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                    }
                                }
                                else
                                {
                                    if (initialization.Parameters["uniqueid"] != null)
                                    {
                                        ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                        pluginCollection.playerUseItemWithCreaturePlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                    }
                                    else if (initialization.Parameters["actionid"] != null)
                                    {
                                        ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                        pluginCollection.playerUseItemWithCreaturePlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                    }
                                    else if (initialization.Parameters["opentibiaid"] != null)
                                    {
                                        ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                        pluginCollection.playerUseItemWithCreaturePlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                    }
                                }
                            }
                            break;

                            case "PlayerMoveItem":
                            {
                                if (initialization.Parameters["uniqueid"] != null)
                                {
                                    ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                    pluginCollection.playerMoveItemPlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["actionid"] != null)
                                {
                                    ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                    pluginCollection.playerMoveItemPlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["opentibiaid"] != null)
                                {
                                    ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                    pluginCollection.playerMoveItemPlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                }
                            }
                            break;

                            case "PlayerMoveCreature":
                            {
                                string name = LuaScope.GetString(initialization.Parameters["name"] );

                                pluginCollection.playerMoveCreaturePlugins.AddPlugin(name, script, initialization.Parameters);
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
                                if (initialization.Parameters["uniqueid"] != null)
                                {
                                    ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                    pluginCollection.creatureStepInPlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["actionid"] != null)
                                {
                                    ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                    pluginCollection.creatureStepInPlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["opentibiaid"] != null)
                                {
                                    ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                    pluginCollection.creatureStepInPlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                }
                            }
                            break;

                            case "CreatureStepOut":
                            {
                                if (initialization.Parameters["uniqueid"] != null)
                                {
                                    ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                    pluginCollection.creatureStepOutPlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["actionid"] != null)
                                {
                                    ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                    pluginCollection.creatureStepOutPlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["opentibiaid"] != null)
                                {
                                    ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                    pluginCollection.creatureStepOutPlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                }
                            }
                            break;

                            case "InventoryEquip":
                            {
                                if (initialization.Parameters["uniqueid"] != null)
                                {
                                    ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                    pluginCollection.inventoryEquipPlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["actionid"] != null)
                                {
                                    ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                    pluginCollection.inventoryEquipPlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["opentibiaid"] != null)
                                {
                                    ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                    pluginCollection.inventoryEquipPlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                }
                            }
                            break;

                            case "InventoryDeEquip":
                            {
                                if (initialization.Parameters["uniqueid"] != null)
                                {
                                    ushort uniqueid = LuaScope.GetUInt16(initialization.Parameters["uniqueid"] );

                                    pluginCollection.inventoryDeEquipPlugins.UniqueIdAddPlugin(uniqueid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["actionid"] != null)
                                {
                                    ushort actionid = LuaScope.GetUInt16(initialization.Parameters["actionid"] );

                                    pluginCollection.inventoryDeEquipPlugins.ActionIdAddPlugin(actionid, script, initialization.Parameters);
                                }
                                else if (initialization.Parameters["opentibiaid"] != null)
                                {
                                    ushort opentibiaid = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] );

                                    pluginCollection.inventoryDeEquipPlugins.OpenTibiaIdAddPlugin(opentibiaid, script, initialization.Parameters);
                                }
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

                                pluginCollection.playerSayPlugins.AddPlugin(message, script, initialization.Parameters);
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
                                pluginCollection.playerLoginPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "PlayerLogout":
                            {
                                pluginCollection.playerLogoutPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "PlayerAdvanceLevel":
                            {
                                pluginCollection.playerAdvanceLevelPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "PlayerAdvanceSkill":
                            {
                                pluginCollection.playerAdvanceSkillPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "CreatureDeath":
                            {
                                pluginCollection.creatureDeathPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "CreatureKill":
                            {
                                pluginCollection.creatureKillPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "PlayerEarnAchievement":
                            {
                                pluginCollection.playerEarnAchievementPlugins.AddPlugin(script, initialization.Parameters);
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
                                pluginCollection.serverStartupPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "ServerShutdown":
                            {
                                pluginCollection.serverShutdownPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "ServerSave":
                            {
                                pluginCollection.serverSavePlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;

                            case "ServerRecord":
                            {
                                pluginCollection.serverRecordPlugins.AddPlugin(script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "items")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "ItemCreation":
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(initialization.Parameters["openTibiaId"] );

                                pluginCollection.itemCreationPlugins.AddPlugin(openTibiaId, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "monsters")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "MonsterCreation":
                            {
                                string name = LuaScope.GetString(initialization.Parameters["name"] );

                                pluginCollection.monsterCreationPlugins.AddPlugin(name, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "npcs")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "NpcCreation":
                            {
                                string name = LuaScope.GetString(initialization.Parameters["name"] );

                                pluginCollection.npcCreationPlugins.AddPlugin(name, script, initialization.Parameters);
                            }
                            break;

                            case "Dialogue":
                            {
                                string name = LuaScope.GetString(initialization.Parameters["name"] );

                                pluginCollection.dialoguePlugins.AddPlugin(name, script, initialization.Parameters);
                            }
                            break;
                        }
                    }
                    else if (initialization.Type == "players")
                    {
                        string type = LuaScope.GetString(initialization.Parameters["type"] );

                        switch (type)
                        {
                            case "PlayerCreation":
                            {
                                string name = LuaScope.GetString(initialization.Parameters["name"] );

                                pluginCollection.playerCreationPlugins.AddPlugin(name, script, initialization.Parameters);
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

                        pluginCollection.spells.Add(spell);

                        if (requiresTarget)
                        {
                            pluginCollection.spellPluginsRequiresTarget.AddPlugin(spell.Words, script, initialization.Parameters, spell);
                        }
                        else
                        {
                            pluginCollection.spellPlugins.AddPlugin(spell.Words, script, initialization.Parameters, spell);
                        }
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

                        pluginCollection.runes.Add(rune);

                        if (requiresTarget)
                        {
                            pluginCollection.runePluginsRequiresTarget.AddPlugin(rune.OpenTibiaId, script, initialization.Parameters, rune);
                        }
                        else
                        {
                            pluginCollection.runePlugins.AddPlugin(rune.OpenTibiaId, script, initialization.Parameters, rune);
                        }
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

                        pluginCollection.weapons.Add(weapon);

                        pluginCollection.weaponPlugins.AddPlugin(weapon.OpenTibiaId, script, initialization.Parameters, weapon);
                    }
                    else if (initialization.Type == "ammunitions")
                    {
                        Ammunition ammunition = new Ammunition()
                        {
                            OpenTibiaId = LuaScope.GetUInt16(initialization.Parameters["opentibiaid"] ),

                            Level = LuaScope.GetInt32(initialization.Parameters["level"] )
                        };

                        pluginCollection.ammunitions.Add(ammunition);

                        pluginCollection.ammunitionPlugins.AddPlugin(ammunition.OpenTibiaId, script, initialization.Parameters, ammunition);
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

                        pluginCollection.raids.Add(raid);

                        pluginCollection.raidPlugins.AddPlugin(raid.Name, script, initialization.Parameters, raid);
                    }
                    else if (initialization.Type == "monsterattacks")
                    {
                        string name = LuaScope.GetString(initialization.Parameters["name"] );

                        pluginCollection.monsterAttackPlugins.AddPlugin(name, script, initialization.Parameters);
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

            this.playerRotateItemPlugins = new ItemPluginDictionaryCached<PlayerRotateItemPlugin, LuaScriptingPlayerRotateItemPlugin>(this);

            this.playerUseItemPlugins = new ItemPluginDictionaryCached<PlayerUseItemPlugin, LuaScriptingPlayerUseItemPlugin>(this);

            this.playerUseItemWithItemPluginsAllowFarUse = new ItemPluginDictionaryCached<PlayerUseItemWithItemPlugin, LuaScriptingPlayerUseItemWithItemPlugin>(this);
            this.playerUseItemWithItemPlugins = new ItemPluginDictionaryCached<PlayerUseItemWithItemPlugin, LuaScriptingPlayerUseItemWithItemPlugin>(this);

            this.playerUseItemWithCreaturePluginsAllowFarUse = new ItemPluginDictionaryCached<PlayerUseItemWithCreaturePlugin, LuaScriptingPlayerUseItemWithCreaturePlugin>(this);
            this.playerUseItemWithCreaturePlugins = new ItemPluginDictionaryCached<PlayerUseItemWithCreaturePlugin, LuaScriptingPlayerUseItemWithCreaturePlugin>(this);
    
            this.playerMoveCreaturePlugins = new PluginDictionaryCached<string, PlayerMoveCreaturePlugin, LuaScriptingPlayerMoveCreaturePlugin>(this);
     
            this.playerMoveItemPlugins = new ItemPluginDictionaryCached<PlayerMoveItemPlugin, LuaScriptingPlayerMoveItemPlugin>(this);
   
            this.creatureStepInPlugins = new ItemPluginDictionaryCached<CreatureStepInPlugin, LuaScriptingCreatureStepInPlugin>(this);
 
            this.creatureStepOutPlugins = new ItemPluginDictionaryCached<CreatureStepOutPlugin, LuaScriptingCreatureStepOutPlugin>(this);
   
            this.inventoryEquipPlugins = new ItemPluginDictionaryCached<InventoryEquipPlugin, LuaScriptingInventoryEquipPlugin>(this);
       
            this.inventoryDeEquipPlugins = new ItemPluginDictionaryCached<InventoryDeEquipPlugin, LuaScriptingInventoryDeEquipPlugin>(this);
           
            this.playerSayPlugins = new PluginDictionaryCached<string, PlayerSayPlugin, LuaScriptingPlayerSayPlugin>(this);
   
            this.playerLoginPlugins = new PluginListCached<PlayerLoginPlugin, LuaScriptingPlayerLoginPlugin>(this);
    
            this.playerLogoutPlugins = new PluginListCached<PlayerLogoutPlugin, LuaScriptingPlayerLogoutPlugin>(this);
   
            this.playerAdvanceLevelPlugins = new PluginListCached<PlayerAdvanceLevelPlugin, LuaScriptingPlayerAdvanceLevelPlugin>(this);
     
            this.playerAdvanceSkillPlugins = new PluginListCached<PlayerAdvanceSkillPlugin, LuaScriptingPlayerAdvanceSkillPlugin>(this);
      
            this.creatureDeathPlugins = new PluginListCached<CreatureDeathPlugin, LuaScriptingCreatureDeathPlugin>(this);

            this.creatureKillPlugins = new PluginListCached<CreatureKillPlugin, LuaScriptingCreatureKillPlugin>(this);
      
            this.playerEarnAchievementPlugins = new PluginListCached<PlayerEarnAchievementPlugin, LuaScriptingPlayerEarnAchievementPlugin>(this);
  
            this.serverStartupPlugins = new PluginListCached<ServerStartupPlugin, LuaScriptingServerStartupPlugin>(this);
  
            this.serverShutdownPlugins = new PluginListCached<ServerShutdownPlugin, LuaScriptingServerShutdownPlugin>(this);
            
            this.serverSavePlugins = new PluginListCached<ServerSavePlugin, LuaScriptingServerSavePlugin>(this);
     
            this.serverRecordPlugins = new PluginListCached<ServerRecordPlugin, LuaScriptingServerRecordPlugin>(this);
     
            this.dialoguePlugins = new PluginDictionary<string, DialoguePlugin, LuaScriptingDialoguePlugin>(this);
     
            this.itemCreationPlugins = new PluginDictionaryCached<ushort, ItemCreationPlugin, LuaScriptingItemCreationPlugin>(this);
    
            this.monsterCreationPlugins = new PluginDictionaryCached<string, MonsterCreationPlugin, LuaScriptingMonsterCreationPlugin>(this);
     
            this.npcCreationPlugins = new PluginDictionaryCached<string, NpcCreationPlugin, LuaScriptingNpcCreationPlugin>(this);
   
            this.playerCreationPlugins = new PluginDictionaryCached<string, PlayerCreationPlugin, LuaScriptingPlayerCreationPlugin>(this);
   
            this.spellPluginsRequiresTarget = new PluginDictionaryCached<string, SpellPlugin, LuaScriptingSpellPlugin>(this);
            this.spellPlugins = new PluginDictionaryCached<string, SpellPlugin, LuaScriptingSpellPlugin>(this);

            this.runePluginsRequiresTarget = new PluginDictionaryCached<ushort, RunePlugin, LuaScriptingRunePlugin>(this);
            this.runePlugins = new PluginDictionaryCached<ushort, RunePlugin, LuaScriptingRunePlugin>(this);

            this.weaponPlugins = new PluginDictionaryCached<ushort, WeaponPlugin, LuaScriptingWeaponPlugin>(this);

            this.ammunitionPlugins = new PluginDictionaryCached<ushort, AmmunitionPlugin, LuaScriptingAmmunitionPlugin>(this);

            this.raidPlugins = new PluginDictionaryCached<string, RaidPlugin, LuaScriptingRaidPlugin>(this);

            this.monsterAttackPlugins = new PluginDictionaryCached<string, MonsterAttackPlugin, LuaScriptingMonsterAttackPlugin>(this);
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
                        if (plugin["uniqueid"] != null)
                        {
                            ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                            playerRotateItemPlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                        }
                        else if (plugin["actionid"] != null)
                        {
                            ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                            playerRotateItemPlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                        }
                        else if (plugin["opentibiaid"] != null)
                        {
                            ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                            playerRotateItemPlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                        }
                    }
                    break;

                    case "PlayerUseItem":
                    {
                        if (plugin["uniqueid"] != null)
                        {
                            ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                            playerUseItemPlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                        }
                        else if (plugin["actionid"] != null)
                        {
                            ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                            playerUseItemPlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                        }
                        else if (plugin["opentibiaid"] != null)
                        {
                            ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                            playerUseItemPlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                        }
                    }
                    break;

                    case "PlayerUseItemWithItem":
                    {
                        bool allowFarUse = LuaScope.GetBoolean(plugin["allowfaruse"] );

                        if (allowFarUse)
                        {
                            if (plugin["uniqueid"] != null)
                            {
                                ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                                playerUseItemWithItemPluginsAllowFarUse.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                            }
                            else if (plugin["actionid"] != null)
                            {
                                ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                                playerUseItemWithItemPluginsAllowFarUse.ActionIdAddPlugin(actionid, fileName, plugin);
                            }
                            else if (plugin["opentibiaid"] != null)
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                                playerUseItemWithItemPluginsAllowFarUse.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                            }
                        }
                        else
                        {
                            if (plugin["uniqueid"] != null)
                            {
                                ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                                playerUseItemWithItemPlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                            }
                            else if (plugin["actionid"] != null)
                            {
                                ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                                playerUseItemWithItemPlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                            }
                            else if (plugin["opentibiaid"] != null)
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                                playerUseItemWithItemPlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                            }
                        }
                    }
                    break;

                    case "PlayerUseItemWithCreature":
                    {
                        bool allowFarUse = LuaScope.GetBoolean(plugin["allowfaruse"] );

                        if (allowFarUse)
                        {
                            if (plugin["uniqueid"] != null)
                            {
                                ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                                playerUseItemWithCreaturePluginsAllowFarUse.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                            }
                            else if (plugin["actionid"] != null)
                            {
                                ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                                playerUseItemWithCreaturePluginsAllowFarUse.ActionIdAddPlugin(actionid, fileName, plugin);
                            }
                            else if (plugin["opentibiaid"] != null)
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                                playerUseItemWithCreaturePluginsAllowFarUse.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                            }
                        }
                        else
                        {
                            if (plugin["uniqueid"] != null)
                            {
                                ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                                playerUseItemWithCreaturePlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                            }
                            else if (plugin["actionid"] != null)
                            {
                                ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                                playerUseItemWithCreaturePlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                            }
                            else if (plugin["opentibiaid"] != null)
                            {
                                ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                                playerUseItemWithCreaturePlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                            }
                        }
                    }
                    break;

                    case "PlayerMoveItem":
                    {
                        if (plugin["uniqueid"] != null)
                        {
                            ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                            playerMoveItemPlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                        }
                        else if (plugin["actionid"] != null)
                        {
                            ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                            playerMoveItemPlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                        }
                        else if (plugin["opentibiaid"] != null)
                        {
                            ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                            playerMoveItemPlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                        }
                    }
                    break;

                    case "PlayerMoveCreature":
                    {
                        string name = LuaScope.GetString(plugin["name"] );

                        playerMoveCreaturePlugins.AddPlugin(name, fileName, plugin);
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
                        if (plugin["uniqueid"] != null)
                        {
                            ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                            creatureStepInPlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                        }
                        else if (plugin["actionid"] != null)
                        {
                            ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                            creatureStepInPlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                        }
                        else if (plugin["opentibiaid"] != null)
                        {
                            ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                            creatureStepInPlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                        }
                    }
                    break;

                    case "CreatureStepOut":
                    {
                        if (plugin["uniqueid"] != null)
                        {
                            ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                            creatureStepOutPlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                        }
                        else if (plugin["actionid"] != null)
                        {
                            ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                            creatureStepOutPlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                        }
                        else if (plugin["opentibiaid"] != null)
                        {
                            ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                            creatureStepOutPlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                        }
                    }
                    break;

                    case "InventoryEquip":
                    {
                        if (plugin["uniqueid"] != null)
                        {
                            ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                            inventoryEquipPlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                        }
                        else if (plugin["actionid"] != null)
                        {
                            ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                            inventoryEquipPlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                        }
                        else if (plugin["opentibiaid"] != null)
                        {
                            ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                            inventoryEquipPlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                        }
                    }
                    break;

                    case "InventoryDeEquip":
                    {
                        if (plugin["uniqueid"] != null)
                        {
                            ushort uniqueid = LuaScope.GetUInt16(plugin["uniqueid"] );

                            inventoryDeEquipPlugins.UniqueIdAddPlugin(uniqueid, fileName, plugin);
                        }
                        else if (plugin["actionid"] != null)
                        {
                            ushort actionid = LuaScope.GetUInt16(plugin["actionid"] );

                            inventoryDeEquipPlugins.ActionIdAddPlugin(actionid, fileName, plugin);
                        }
                        else if (plugin["opentibiaid"] != null)
                        {
                            ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                            inventoryDeEquipPlugins.OpenTibiaIdAddPlugin(openTibiaId, fileName, plugin);
                        }
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

                        playerSayPlugins.AddPlugin(message, fileName, plugin);
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
                        playerLoginPlugins.AddPlugin(fileName, plugin);
                    }
                    break;

                    case "PlayerLogout":
                    {
                        playerLogoutPlugins.AddPlugin(fileName, plugin);
                    }
                    break;

                    case "PlayerAdvanceLevel":
                    {
                        playerAdvanceLevelPlugins.AddPlugin(fileName, plugin);
                    }
                    break;

                    case "PlayerAdvanceSkill":
                    {
                        playerAdvanceSkillPlugins.AddPlugin(fileName, plugin);
                    }
                    break;

                    case "CreatureDeath":
                    {
                        creatureDeathPlugins.AddPlugin(fileName, plugin);
                    }
                    break;

                    case "CreatureKill":
                    {
                        creatureKillPlugins.AddPlugin(fileName, plugin);
                    }
                    break;

                    case "PlayerEarnAchievement":
                    {
                        playerEarnAchievementPlugins.AddPlugin(fileName, plugin);
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
                        serverStartupPlugins.AddPlugin(fileName, plugin);
                    }
                    break;

                    case "ServerShutdown":
                    {
                        serverShutdownPlugins.AddPlugin(fileName, plugin);
                    }
                    break;

                    case "ServerSave":
                    {
                        serverSavePlugins.AddPlugin(fileName, plugin);
                    }
                    break;
                        
                    case "ServerRecord":
                    {
                        serverRecordPlugins.AddPlugin(fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.items"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "ItemCreation":
                    {
                        ushort openTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] );

                        itemCreationPlugins.AddPlugin(openTibiaId, fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.monsters"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "MonsterCreation":
                    {
                        string name = LuaScope.GetString(plugin["name"] );

                        monsterCreationPlugins.AddPlugin(name, fileName, plugin);
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
                    case "NpcCreation":
                    {
                        string name = LuaScope.GetString(plugin["name"] );

                        npcCreationPlugins.AddPlugin(name, fileName, plugin);
                    }
                    break;

                    case "Dialogue":
                    {
                        string name = LuaScope.GetString(plugin["name"] );

                        dialoguePlugins.AddPlugin(name, fileName, plugin);
                    }
                    break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.players"] ).Values)
            {
                string type = LuaScope.GetString(plugin["type"] );

                string fileName = LuaScope.GetString(plugin["filename"] );

                switch (type)
                {
                    case "PlayerCreation":
                    {
                        string name = LuaScope.GetString(plugin["name"] );

                        playerCreationPlugins.AddPlugin(name, fileName, plugin);
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

                spells.Add(spell);

                if (requiresTarget)
                {
                    spellPluginsRequiresTarget.AddPlugin(spell.Words, fileName, plugin, spell);
                }
                else
                {
                    spellPlugins.AddPlugin(spell.Words, fileName, plugin, spell);
                }
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

                runes.Add(rune);

                if (requiresTarget)
                {
                    runePluginsRequiresTarget.AddPlugin(rune.OpenTibiaId, fileName, plugin, rune);
                }
                else
                {
                    runePlugins.AddPlugin(rune.OpenTibiaId, fileName, plugin, rune);
                }
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

                weapons.Add(weapon);

                weaponPlugins.AddPlugin(weapon.OpenTibiaId, fileName, plugin, weapon);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.ammunitions"] ).Values)
            {
                string fileName = LuaScope.GetString(plugin["filename"] );

                Ammunition ammunition = new Ammunition()
                {
                    OpenTibiaId = LuaScope.GetUInt16(plugin["opentibiaid"] ),

                    Level = LuaScope.GetInt32(plugin["level"])
                };

                ammunitions.Add(ammunition);

                ammunitionPlugins.AddPlugin(ammunition.OpenTibiaId, fileName, plugin, ammunition);
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

                raids.Add(raid);

                raidPlugins.AddPlugin(raid.Name, fileName, plugin, raid);
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.monsterattacks"] ).Values)
            {
                string fileName = LuaScope.GetString(plugin["filename"] );

                string name = LuaScope.GetString(plugin["name"] );

                monsterAttackPlugins.AddPlugin(name, fileName, plugin);
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