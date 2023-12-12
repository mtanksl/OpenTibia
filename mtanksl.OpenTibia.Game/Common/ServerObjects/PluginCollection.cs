using OpenTibia.Game.Plugins;
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
            private Dictionary<TKey, TValue> plugins = new Dictionary<TKey, TValue>();

            public void AddPlugin(TKey key, Func<TValue> factory)
            {
                TValue plugin = factory();

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

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "PlayerRotateItem":

                        if (fileName.EndsWith(".lua") )
                        {
                            playerRotateItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerRotateItemPlugin("data/plugins/actions/" + fileName) );
                        }
                        else
                        {
                            playerRotateItemPlugins.AddPlugin(openTibiaId, () => (PlayerRotateItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                        }
                    
                        break;

                    case "PlayerUseItem":

                        if (fileName.EndsWith(".lua") )
                        {
                            playerUseItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemPlugin("data/plugins/actions/" + fileName) );
                        }
                        else
                        {
                            playerUseItemPlugins.AddPlugin(openTibiaId, () => (PlayerUseItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                        }
                  
                        break;

                    case "PlayerUseItemWithItem":
                        { 
                            bool allowFarUse = (bool)plugin["allowfaruse"];

                            if (fileName.EndsWith(".lua") )
                            {
                                if (allowFarUse)
                                {
                                    playerUseItemWithItemPluginsAllowFarUse.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithItemPlugin("data/plugins/actions/" + fileName) );
                                }
                                else
                                {
                                    playerUseItemWithItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithItemPlugin("data/plugins/actions/" + fileName) );
                                }
                            }
                            else
                            {
                                if (allowFarUse)
                                {
                                    playerUseItemWithItemPluginsAllowFarUse.AddPlugin(openTibiaId, () => (PlayerUseItemWithItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                                }
                                else
                                {
                                    playerUseItemWithItemPlugins.AddPlugin(openTibiaId, () => (PlayerUseItemWithItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                                }
                            }
                        }
                        break;

                    case "PlayerUseItemWithCreature":
                        { 
                            bool allowFarUse = (bool)plugin["allowfaruse"];

                            if (fileName.EndsWith(".lua"))
                            {
                                if (allowFarUse)
                                {
                                    playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin("data/plugins/actions/" + fileName) );
                                }
                                else
                                {
                                    playerUseItemWithCreaturePlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin("data/plugins/actions/" + fileName) );
                                }
                            }
                            else
                            {
                                if (allowFarUse)
                                {
                                    playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(openTibiaId, () => (PlayerUseItemWithCreaturePlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                                }
                                else
                                {
                                    playerUseItemWithCreaturePlugins.AddPlugin(openTibiaId, () => (PlayerUseItemWithCreaturePlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                                }
                            }
                        }
                        break;

                    case "PlayerMoveItem":
                        
                        if (fileName.EndsWith(".lua") )
                        {
                            playerMoveItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerMoveItemPlugin("data/plugins/actions/" + fileName) );
                        }
                        else
                        {
                            playerMoveItemPlugins.AddPlugin(openTibiaId, () => (PlayerMoveItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                        }

                        break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.movements"] ).Values)
            {
                string type = (string)plugin["type"];

                ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "CreatureStepIn":

                        if (fileName.EndsWith(".lua") )
                        {
                            creatureStepInPlugins.AddPlugin(openTibiaId, () => new LuaScriptingCreatureStepInPlugin("data/plugins/movements/" + fileName) );
                        }
                        else
                        {
                            creatureStepInPlugins.AddPlugin(openTibiaId, () => (CreatureStepInPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                        }

                        break;

                    case "CreatureStepOut":

                        if (fileName.EndsWith(".lua") )
                        {
                            creatureStepOutPlugins.AddPlugin(openTibiaId, () => new LuaScriptingCreatureStepOutPlugin("data/plugins/movements/" + fileName) );
                        }
                        else
                        {
                            creatureStepOutPlugins.AddPlugin(openTibiaId, () => (CreatureStepOutPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                        }

                        break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.talkactions"] ).Values)
            {
                string type = (string)plugin["type"];

                string message = (string)plugin["message"];

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "PlayerSay":

                        if (fileName.EndsWith(".lua") )
                        {
                            playerSayPlugins.AddPlugin(message, () => new LuaScriptingPlayerSayPlugin("data/plugins/talkactions/" + fileName) );
                        }
                        else
                        {
                            playerSayPlugins.AddPlugin(message, () => (PlayerSayPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                        }
                    
                        break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.npcs"] ).Values)
            {
                string type = (string)plugin["type"];

                string name = (string)plugin["name"];

                string fileName = (string)plugin["filename"];

                switch (type)
                {
                    case "Dialogue":

                        if (fileName.EndsWith(".lua") )
                        {
                            dialoguePlugins.AddPlugin(name, () => new LuaScriptingDialoguePlugin("data/plugins/npcs/" + fileName) );
                        }
                        else
                        {
                            dialoguePlugins.AddPlugin(name, () => (DialoguePlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                        }
                                       
                        break;
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.spells"] ).Values)
            {
                string words = (string)plugin["words"];

                string fileName = (string)plugin["filename"];

                bool requiresTarget = (bool)plugin["requirestarget"];

                Spell spell = new Spell()
                {
                    Words = words,

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

                spells.Add(spell);

                if (fileName.EndsWith(".lua") )
                {
                    if (requiresTarget)
                    {
                        spellPluginsRequiresTarget.AddPlugin(words, () => new LuaScriptingSpellPlugin("data/plugins/spells/" + fileName, spell) );
                    }
                    else
                    {
                        spellPlugins.AddPlugin(words, () => new LuaScriptingSpellPlugin("data/plugins/spells/" + fileName, spell) );
                    }
                }
                else
                {
#if AOT
                    if (requiresTarget)
                    {
                        spellPluginsRequiresTarget.AddPlugin(words, () => _AotCompilation.SpellPlugins[fileName](spell) );
                    }
                    else
                    {
                        spellPlugins.AddPlugin(words, () => _AotCompilation.SpellPlugins[fileName](spell) );
                    }
#else
                    if (requiresTarget)
                    {
                        spellPluginsRequiresTarget.AddPlugin(words, () => (SpellPlugin)Activator.CreateInstance(Type.GetType(fileName), spell) );
                    }
                    else
                    {
                        spellPlugins.AddPlugin(words, () => (SpellPlugin)Activator.CreateInstance(Type.GetType(fileName), spell) );
                    }
#endif
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.runes"] ).Values)
            {
                ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                string fileName = (string)plugin["filename"];

                bool requiresTarget = (bool)plugin["requirestarget"];

                Rune rune = new Rune()
                {
                    OpenTibiaId = openTibiaId,

                    Name = (string)plugin["name"],

                    Group = (string)plugin["group"],

                    GroupCooldown = TimeSpan.FromSeconds( (int)(long)plugin["groupcooldown"]),

                    Level = (int)(long)plugin["level"],

                    MagicLevel = (int)(long)plugin["magiclevel"]
                };

                runes.Add(rune);

                if (fileName.EndsWith(".lua") )
                {
                    if (requiresTarget)
                    {
                        runePluginsRequiresTarget.AddPlugin(openTibiaId, () => new LuaScriptingRunePlugin("data/plugins/runes/" + fileName, rune) );
                    }
                    else
                    {
                        runePlugins.AddPlugin(openTibiaId, () => new LuaScriptingRunePlugin("data/plugins/runes/" + fileName, rune) );
                    }
                }
                else
                {
#if AOT
                    if (requiresTarget)
                    {
                        runePluginsRequiresTarget.AddPlugin(openTibiaId, () => _AotCompilation.RunePlugins[fileName](rune) );
                    }
                    else
                    {
                        runePlugins.AddPlugin(openTibiaId, () => _AotCompilation.RunePlugins[fileName](rune) );
                    }
#else
                    if (requiresTarget)
                    {
                        runePluginsRequiresTarget.AddPlugin(openTibiaId, () => (RunePlugin)Activator.CreateInstance(Type.GetType(fileName), rune) );
                    }
                    else
                    {
                        runePlugins.AddPlugin(openTibiaId, () => (RunePlugin)Activator.CreateInstance(Type.GetType(fileName), rune) );
                    }
#endif
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.weapons"] ).Values)
            {
                ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                string fileName = (string)plugin["filename"];

                Weapon weapon = new Weapon()
                {
                    OpenTibiaId = openTibiaId,

                    Level = (int)(long)plugin["level"],

                    Mana = (int)(long)plugin["mana"],

                    Vocations = ( (LuaTable)plugin["vocations"]).Values.Cast<long>().Select(v => (Vocation)v ).ToArray()
                };

                weapons.Add(weapon);

                if (fileName.EndsWith(".lua") )
                {
                    weaponPlugins.AddPlugin(openTibiaId, () => new LuaScriptingWeaponPlugin("data/plugins/weapons/" + fileName, weapon) );
                }
                else
                {
#if AOT
                    weaponPlugins.AddPlugin(openTibiaId, () => _AotCompilation.WeaponPlugins[fileName](weapon) );
#else
                    weaponPlugins.AddPlugin(openTibiaId, () => (WeaponPlugin)Activator.CreateInstance(Type.GetType(fileName), weapon) );
#endif                    
                }
            }

            foreach (LuaTable plugin in ( (LuaTable)script["plugins.ammunitions"] ).Values)
            {
                ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                string fileName = (string)plugin["filename"];

                Ammunition ammunition = new Ammunition()
                {
                    OpenTibiaId = openTibiaId
                };

                ammunitions.Add(ammunition);

                if (fileName.EndsWith(".lua") )
                {
                    ammunitionPlugins.AddPlugin(openTibiaId, () => new LuaScriptingAmmunitionPlugin("data/plugins/ammunitions/" + fileName, ammunition) );
                }
                else
                {
#if AOT
                    ammunitionPlugins.AddPlugin(openTibiaId, () => _AotCompilation.AmmunitionPlugins[fileName](ammunition) );
#else
                    ammunitionPlugins.AddPlugin(openTibiaId, () => (AmmunitionPlugin)Activator.CreateInstance(Type.GetType(fileName), ammunition) );
#endif
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

        private PluginDictionaryCached<string, SpellPlugin> spellPluginsRequiresTarget = new PluginDictionaryCached<string, SpellPlugin>();

        private PluginDictionaryCached<string, SpellPlugin> spellPlugins = new PluginDictionaryCached<string, SpellPlugin>();

        public SpellPlugin GetSpellPlugin(bool requirestarget, string words)
        {
            if (requirestarget)
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

        public RunePlugin GetRunePlugin(bool requirestarget, ushort openTibiaId)
        {
            if (requirestarget)
            {
                return runePluginsRequiresTarget.GetPlugin(openTibiaId);
            }
            else
            {
                return runePlugins.GetPlugin(openTibiaId);
            }
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

                playerMoveItemPlugins.GetPlugins(),

                creatureStepInPlugins.GetPlugins(),

                creatureStepOutPlugins.GetPlugins(),

                playerSayPlugins.GetPlugins(),

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
            script.Dispose();
        }
    }
}