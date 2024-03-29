using NLua;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
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

        public class AutoLoadPlugin : IDisposable
        {
            private LuaScope script;

            public AutoLoadPlugin(PluginCollection pluginCollection, string filePath)
            {
                LuaScope scripts = pluginCollection.server.LuaScripts.LoadLib(
                    pluginCollection.server.PathResolver.GetFullPath("data/plugins/scripts/lib.lua"), 
                    pluginCollection.server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    pluginCollection.server.PathResolver.GetFullPath("data/lib.lua") );

                var initialization = new List<(string Type, LuaTable Parameters)>();

                scripts["registerplugin"] = (string type, LuaTable parameters) => 
                {
                    initialization.Add( (type, parameters) );
                };

                script = pluginCollection.server.LuaScripts.LoadScript(pluginCollection.server.PathResolver.GetFullPath(filePath), scripts);

                foreach (var item in initialization)
                {
                    if (item.Type == "actions")
                    {
                        string type = (string)item.Parameters["type"];

                        switch (type)
                        {
                            case "PlayerRotateItem":
                            {
                                ushort openTibiaId = (ushort)(long)item.Parameters["opentibiaid"];

                                pluginCollection.playerRotateItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerRotateItemPlugin(script, item.Parameters) );
                            }
                            break;

                            case "PlayerUseItem":
                            {
                                ushort openTibiaId = (ushort)(long)item.Parameters["opentibiaid"];

                                pluginCollection.playerUseItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemPlugin(script, item.Parameters) );
                            }
                            break;

                            case "PlayerUseItemWithItem":
                            {
                                ushort openTibiaId = (ushort)(long)item.Parameters["opentibiaid"];

                                bool allowFarUse = (bool)item.Parameters["allowfaruse"];

                                if (allowFarUse)
                                {
                                    pluginCollection.playerUseItemWithItemPluginsAllowFarUse.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithItemPlugin(script, item.Parameters) );
                                }
                                else
                                {
                                    pluginCollection.playerUseItemWithItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithItemPlugin(script, item.Parameters) );
                                }                       
                            }
                            break;

                            case "PlayerUseItemWithCreature":
                            {
                                ushort openTibiaId = (ushort)(long)item.Parameters["opentibiaid"];

                                bool allowFarUse = (bool)item.Parameters["allowfaruse"];

                                if (allowFarUse)
                                {
                                    pluginCollection.playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin(script, item.Parameters) );
                                }
                                else
                                {
                                    pluginCollection.playerUseItemWithCreaturePlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin(script, item.Parameters) );
                                }
                            }
                            break;

                            case "PlayerMoveItem":
                            {
                                ushort openTibiaId = (ushort)(long)item.Parameters["opentibiaid"];

                                pluginCollection.playerMoveItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerMoveItemPlugin(script, item.Parameters) );

                            }
                            break;

                            case "PlayerMoveCreature":
                            {
                                string name = (string)item.Parameters["name"];

                                pluginCollection.playerMoveCreaturePlugins.AddPlugin(name, () => new LuaScriptingPlayerMoveCreaturePlugin(script, item.Parameters) );
                            }
                            break;
                        }
                    }
                    else if (item.Type == "movements")
                    {
                        string type = (string)item.Parameters["type"];

                        switch (type)
                        {
                            case "CreatureStepIn":
                            {
                                ushort openTibiaId = (ushort)(long)item.Parameters["opentibiaid"];

                                pluginCollection.creatureStepInPlugins.AddPlugin(openTibiaId, () => new LuaScriptingCreatureStepInPlugin(script, item.Parameters) );
                            }
                            break;

                            case "CreatureStepOut":
                            {
                                ushort openTibiaId = (ushort)(long)item.Parameters["opentibiaid"];

                                pluginCollection.creatureStepOutPlugins.AddPlugin(openTibiaId, () => new LuaScriptingCreatureStepOutPlugin(script, item.Parameters) );
                            }
                            break;
                        }
                    }
                    else if (item.Type == "talkactions")
                    {
                        string type = (string)item.Parameters["type"];

                        switch (type)
                        {
                            case "PlayerSay":
                            {
                                string message = (string)item.Parameters["message"];

                                pluginCollection.playerSayPlugins.AddPlugin(message, () => new LuaScriptingPlayerSayPlugin(script, item.Parameters) );
                            }
                            break;
                        }
                    }
                    else if (item.Type == "npcs")
                    {
                        string type = (string)item.Parameters["type"];

                        switch (type)
                        {
                            case "Dialogue":
                            {
                                string name = (string)item.Parameters["name"];

                                pluginCollection.dialoguePlugins.AddPlugin(name, () => new LuaScriptingDialoguePlugin(script, item.Parameters) );
                            }
                            break;
                        }
                    }
                    else if (item.Type == "spells")
                    {
                        //TODO
                    }
                    else if (item.Type == "runes")
                    {
                        //TODO
                    }
                    else if (item.Type == "weapons")
                    {
                        //TODO
                    }
                    else if (item.Type == "ammunitions")
                    {
                        //TODO
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

                        if (fileName.EndsWith(".lua") )
                        {
                            playerRotateItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerRotateItemPlugin(fileName) );
                        }
                        else
                        {
#if AOT
                            playerRotateItemPlugins.AddPlugin(openTibiaId, () => (PlayerRotateItemPlugin)_AotCompilation.OtherPlugins[fileName]() );
#else
                            playerRotateItemPlugins.AddPlugin(openTibiaId, () => (PlayerRotateItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
#endif 
                        }
                    }
                    break;

                    case "PlayerUseItem":
                    { 
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        if (fileName.EndsWith(".lua") )
                        {
                            playerUseItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemPlugin(fileName) );
                        }
                        else
                        {
#if AOT
                            playerUseItemPlugins.AddPlugin(openTibiaId, () => (PlayerUseItemPlugin)_AotCompilation.OtherPlugins[fileName]() );
#else
                            playerUseItemPlugins.AddPlugin(openTibiaId, () => (PlayerUseItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
#endif
                        }
                    }
                    break;

                    case "PlayerUseItemWithItem":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        bool allowFarUse = (bool)plugin["allowfaruse"];

                        if (fileName.EndsWith(".lua") )
                        {
                            if (allowFarUse)
                            {
                                playerUseItemWithItemPluginsAllowFarUse.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithItemPlugin(fileName) );
                            }
                            else
                            {
                                playerUseItemWithItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithItemPlugin(fileName) );
                            }
                        }
                        else
                        {
#if AOT
                            if (allowFarUse)
                            {
                                playerUseItemWithItemPluginsAllowFarUse.AddPlugin(openTibiaId, () => (PlayerUseItemWithItemPlugin)_AotCompilation.OtherPlugins[fileName]() );
                            }
                            else
                            {
                                playerUseItemWithItemPlugins.AddPlugin(openTibiaId, () => (PlayerUseItemWithItemPlugin)_AotCompilation.OtherPlugins[fileName]() );
                            }
#else
                            if (allowFarUse)
                            {
                                playerUseItemWithItemPluginsAllowFarUse.AddPlugin(openTibiaId, () => (PlayerUseItemWithItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                            }
                            else
                            {
                                playerUseItemWithItemPlugins.AddPlugin(openTibiaId, () => (PlayerUseItemWithItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                            }
#endif
                        }
                    }
                    break;

                    case "PlayerUseItemWithCreature":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        bool allowFarUse = (bool)plugin["allowfaruse"];

                        if (fileName.EndsWith(".lua") )
                        {
                            if (allowFarUse)
                            {
                                playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin(fileName) );
                            }
                            else
                            {
                                playerUseItemWithCreaturePlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerUseItemWithCreaturePlugin(fileName) );
                            }
                        }
                        else
                        {
#if AOT
                            if (allowFarUse)
                            {
                                playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(openTibiaId, () => (PlayerUseItemWithCreaturePlugin)_AotCompilation.OtherPlugins[fileName]() );
                            }
                            else
                            {
                                playerUseItemWithCreaturePlugins.AddPlugin(openTibiaId, () => (PlayerUseItemWithCreaturePlugin)_AotCompilation.OtherPlugins[fileName]() );
                            }
#else
                            if (allowFarUse)
                            {
                                playerUseItemWithCreaturePluginsAllowFarUse.AddPlugin(openTibiaId, () => (PlayerUseItemWithCreaturePlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                            }
                            else
                            {
                                playerUseItemWithCreaturePlugins.AddPlugin(openTibiaId, () => (PlayerUseItemWithCreaturePlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
                            }
#endif
                        }
                    }
                    break;

                    case "PlayerMoveItem":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        if (fileName.EndsWith(".lua") )
                        {
                            playerMoveItemPlugins.AddPlugin(openTibiaId, () => new LuaScriptingPlayerMoveItemPlugin(fileName) );
                        }
                        else
                        {
#if AOT
                        playerMoveItemPlugins.AddPlugin(openTibiaId, () => (PlayerMoveItemPlugin)_AotCompilation.OtherPlugins[fileName]() );
#else
                            playerMoveItemPlugins.AddPlugin(openTibiaId, () => (PlayerMoveItemPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
#endif
                        }
                    }
                    break;

                    case "PlayerMoveCreature":
                    {
                        string name = (string)plugin["name"];

                        if (fileName.EndsWith(".lua") )
                        {
                            playerMoveCreaturePlugins.AddPlugin(name, () => new LuaScriptingPlayerMoveCreaturePlugin(fileName) );
                        }
                        else
                        {
#if AOT
                            playerMoveCreaturePlugins.AddPlugin(name, () => (PlayerMoveCreaturePlugin)_AotCompilation.OtherPlugins[fileName]() );
#else
                            playerMoveCreaturePlugins.AddPlugin(name, () => (PlayerMoveCreaturePlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
#endif
                        }
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

                        if (fileName.EndsWith(".lua") )
                        {
                            creatureStepInPlugins.AddPlugin(openTibiaId, () => new LuaScriptingCreatureStepInPlugin(fileName) );
                        }
                        else
                        {
#if AOT
                            creatureStepInPlugins.AddPlugin(openTibiaId, () => (CreatureStepInPlugin)_AotCompilation.OtherPlugins[fileName]() );
#else
                            creatureStepInPlugins.AddPlugin(openTibiaId, () => (CreatureStepInPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
#endif
                        }
                    }
                    break;

                    case "CreatureStepOut":
                    {
                        ushort openTibiaId = (ushort)(long)plugin["opentibiaid"];

                        if (fileName.EndsWith(".lua") )
                        {
                            creatureStepOutPlugins.AddPlugin(openTibiaId, () => new LuaScriptingCreatureStepOutPlugin(fileName) );
                        }
                        else
                        {
#if AOT
                            creatureStepOutPlugins.AddPlugin(openTibiaId, () => (CreatureStepOutPlugin)_AotCompilation.OtherPlugins[fileName]() );
#else
                            creatureStepOutPlugins.AddPlugin(openTibiaId, () => (CreatureStepOutPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
#endif
                        }
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

                        if (fileName.EndsWith(".lua") )
                        {
                            playerSayPlugins.AddPlugin(message, () => new LuaScriptingPlayerSayPlugin(fileName) );
                        }
                        else
                        {
#if AOT
                            playerSayPlugins.AddPlugin(message, () => (PlayerSayPlugin)_AotCompilation.OtherPlugins[fileName]() );
#else
                            playerSayPlugins.AddPlugin(message, () => (PlayerSayPlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
#endif
                        }
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

                        if (fileName.EndsWith(".lua") )
                        {
                            dialoguePlugins.AddPlugin(name, () => new LuaScriptingDialoguePlugin(fileName) );
                        }
                        else
                        {
#if AOT
                            dialoguePlugins.AddPlugin(name, () => (DialoguePlugin)_AotCompilation.OtherPlugins[fileName]() );
#else
                            dialoguePlugins.AddPlugin(name, () => (DialoguePlugin)Activator.CreateInstance(Type.GetType(fileName) ) );
#endif
                        }
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
                        spellPluginsRequiresTarget.AddPlugin(words, () => new LuaScriptingSpellPlugin(fileName, spell) );
                    }
                    else
                    {
                        spellPlugins.AddPlugin(words, () => new LuaScriptingSpellPlugin(fileName, spell) );
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
                        runePluginsRequiresTarget.AddPlugin(openTibiaId, () => new LuaScriptingRunePlugin(fileName, rune) );
                    }
                    else
                    {
                        runePlugins.AddPlugin(openTibiaId, () => new LuaScriptingRunePlugin(fileName, rune) );
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
                    weaponPlugins.AddPlugin(openTibiaId, () => new LuaScriptingWeaponPlugin(fileName, weapon) );
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
                    ammunitionPlugins.AddPlugin(openTibiaId, () => new LuaScriptingAmmunitionPlugin(fileName, ammunition) );
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

        private PluginDictionaryCached<string, PlayerMoveCreaturePlugin> playerMoveCreaturePlugins = new PluginDictionaryCached<string, PlayerMoveCreaturePlugin>();

        public PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(string name)
        {
            return playerMoveCreaturePlugins.GetPlugin(name);
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

                playerMoveCreaturePlugins.GetPlugins(),

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
            foreach (var autoLoadPlugin in autoLoadPlugins)
            {
                autoLoadPlugin.Dispose();
            }

            script.Dispose();
        }
    }
}