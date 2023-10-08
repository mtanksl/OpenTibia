using mtanksl.OpenTibia.Game.Plugins;
using NLua;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;

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
            script = server.LuaScripts.Create(server.PathResolver.GetFullPath("data/plugins/lib.lua"), server.PathResolver.GetFullPath("data/plugins/config.lua") );

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

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.movements"] ).Values)
            {
                string type = (string)plugin["type"];

                ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                string filename = (string)plugin["filename"];

                switch (type)
                {
                    case "CreatureStepIn":

                        creatureStepInPlugins.AddPlugin(openTibiaId, () => new LuaScriptingCreatureStepInPlugin("data/plugins/movements/" + filename) );

                        break;

                    case "CreatureStepOut":

                        creatureStepOutPlugins.AddPlugin(openTibiaId, () => new LuaScriptingCreatureStepOutPlugin("data/plugins/movements/" + filename) );

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

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.spells"] ).Values)
            {
                spellPlugins.AddPlugin( (string)plugin["words"], () => new LuaScriptingSpellPlugin("data/plugins/spells/" + (string)plugin["filename"], new Spell()
                {
                    Name = (string)plugin["name"],

                    Group = (string)plugin["group"],

                    Cooldown = TimeSpan.FromSeconds( (int)(long)plugin["cooldown"] ),

                    GroupCooldown = TimeSpan.FromSeconds( (int)(long)plugin["groupcooldown"] ),

                    Level = (int)(long)plugin["level"],

                    Mana = (int)(long)plugin["mana"],

                    Premium = (bool)plugin["premium"],

                    Vocations = ( (LuaTable)plugin["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                } ) );
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.runes"] ).Values)
            {
                
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.weapons"] ).Values)
            {
                weaponPlugins.AddPlugin( (ushort)(long)plugin["opentibiaid"], () => new LuaScriptingWeaponPlugin("data/plugins/weapons/" + (string)plugin["filename"], new Weapon()
                {
                    Level = (int)(long)plugin["level"],

                    Mana = (int)(long)plugin["mana"],

                    Vocations = ( (LuaTable)plugin["vocations"] ).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                } ) );
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.ammunitions"] ).Values)
            {
                ammunitionPlugins.AddPlugin( (ushort)(long)plugin["opentibiaid"], () => new LuaScriptingAmmunitionPlugin("data/plugins/ammunitions/" + (string)plugin["filename"], new Ammunition()
                {
                    
                } ) );
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

        private PluginDictionaryCached<ushort, CreatureStepInPlugin> creatureStepInPlugins = new PluginDictionaryCached<ushort, CreatureStepInPlugin>();

        public CreatureStepInPlugin GetCreatureStepInPlugin(ushort openTibiaId)
        {
            return creatureStepInPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, CreatureStepOutPlugin> creatureStepOutPlugins = new PluginDictionaryCached<ushort, CreatureStepOutPlugin>();

        public CreatureStepOutPlugin GetCreatureStepOutPlugin(ushort openTibiaId)
        {
            return creatureStepOutPlugins.GetPlugin(openTibiaId);
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

        private PluginDictionaryCached<string, SpellPlugin> spellPlugins = new PluginDictionaryCached<string, SpellPlugin>();

        public SpellPlugin GetSpellPlugin(string words)
        {
            return spellPlugins.GetPlugin(words);
        }

        private PluginDictionaryCached<ushort, WeaponPlugin> weaponPlugins = new PluginDictionaryCached<ushort, WeaponPlugin>();

        public WeaponPlugin GetWeaponPlugin(ushort openTibiaId)
        {
            return weaponPlugins.GetPlugin(openTibiaId);
        }

        private PluginDictionaryCached<ushort, AmmunitionPlugin> ammunitionPlugins = new PluginDictionaryCached<ushort, AmmunitionPlugin>();

        public AmmunitionPlugin GetAmmunitionPlugin(ushort openTibiaId)
        {
            return ammunitionPlugins.GetPlugin(openTibiaId);
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

                playerMoveItemPlugins.GetPlugins(),

                creatureStepInPlugins.GetPlugins(),

                creatureStepOutPlugins.GetPlugins(),

                playerSayPlugins.GetPlugins(),

                dialoguePlugins.GetPlugins(),

                spellPlugins.GetPlugins(),

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
            script.Dispose();
        }
    }
}