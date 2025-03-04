using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPluginCollection : IDisposable
    {
        void Start();

        void ParseActions(string fileName, ILuaScope script, LuaTable parameters);

        void ParseMovements(string fileName, ILuaScope script, LuaTable parameters);

        void ParseTalkActions(string fileName, ILuaScope script, LuaTable parameters);

        void ParseCreatureScripts(string fileName, ILuaScope script, LuaTable parameters);
       
        void ParseCreatureGlobalEvents(string fileName, ILuaScope script, LuaTable parameters);

        void ParseItems(string fileName, ILuaScope script, LuaTable parameters);

        void ParseMonsters(string fileName, ILuaScope script, LuaTable parameters);

        void ParseNpcs(string fileName, ILuaScope script, LuaTable parameters);

        void ParsePlayers(string fileName, ILuaScope script, LuaTable parameters);

        void ParseSpells(string fileName, ILuaScope script, LuaTable parameters);

        void ParseRunes(string fileName, ILuaScope script, LuaTable parameters);

        void ParseWeapons(string fileName, ILuaScope script, LuaTable parameters);

        void ParseAmmunitions(string fileName, ILuaScope script, LuaTable parameters);

        void ParseRaids(string fileName, ILuaScope script, LuaTable parameters);

        void ParseMonsterAttacks(string fileName, ILuaScope script, LuaTable parameters);

        object GetValue(string key);

        PlayerRotateItemPlugin GetPlayerRotateItemPlugin(Item item);

        PlayerUseItemPlugin GetPlayerUseItemPlugin(Item item);

        PlayerUseItemWithItemPlugin GetPlayerUseItemWithItemPlugin(bool allowFarUse, Item item);

        PlayerUseItemWithCreaturePlugin GetPlayerUseItemWithCreaturePlugin(bool allowFarUse, Item item);

        PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(Creature creature);

        PlayerMoveItemPlugin GetPlayerMoveItemPlugin(Item item);

        CreatureStepInPlugin GetCreatureStepInPlugin(Item item);

        CreatureStepOutPlugin GetCreatureStepOutPlugin(Item item);

        InventoryEquipPlugin GetInventoryEquipPlugin(Item item);

        InventoryDeEquipPlugin GetInventoryDeEquipPlugin(Item item);

        PlayerSayPlugin GetPlayerSayPlugin(string message);

        IEnumerable<PlayerLoginPlugin> GetPlayerLoginPlugins();

        IEnumerable<PlayerLogoutPlugin> GetPlayerLogoutPlugins();

        IEnumerable<PlayerAdvanceLevelPlugin> GetPlayerAdvanceLevelPlugins();

        IEnumerable<PlayerAdvanceSkillPlugin> GetPlayerAdvanceSkillPlugins();

        IEnumerable<CreatureDeathPlugin> GetCreatureDeathPlugins();

        IEnumerable<CreatureKillPlugin> GetCreatureKillPlugins();

        IEnumerable<PlayerEarnAchievementPlugin> GetPlayerEarnAchievementPlugins();

        IEnumerable<ServerStartupPlugin> GetServerStartupPlugins();

        IEnumerable<ServerShutdownPlugin> GetServerShutdownPlugins();

        IEnumerable<ServerSavePlugin> GetServerSavePlugins();

        IEnumerable<ServerRecordPlugin> GetServerRecordPlugins();

        DialoguePlugin GetDialoguePlugin(string name);

        ItemCreationPlugin GetItemCreationPlugin(ushort openTibiaId);

        MonsterCreationPlugin GetMonsterCreationPlugin(string name);

        NpcCreationPlugin GetNpcCreationPlugin(string name);

        PlayerCreationPlugin GetPlayerCreationPlugin(string name);

        SpellPlugin GetSpellPlugin(bool requiresTarget, string words);

        RunePlugin GetRunePlugin(bool requiresTarget, ushort openTibiaId);

        WeaponPlugin GetWeaponPlugin(ushort openTibiaId);

        AmmunitionPlugin GetAmmunitionPlugin(ushort openTibiaId);

        RaidPlugin GetRaidPlugin(string name);

        MonsterAttackPlugin GetMonsterAttackPlugin(string name);

        List<Spell> Spells { get; }

        List<Rune> Runes { get; }

        List<Weapon> Weapons { get; }

        List<Ammunition> Ammunitions { get; }

        List<Raid> Raids { get; }

        void Stop();
    }
}