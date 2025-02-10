using NLua;
using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPluginCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        void AddPlayerRotateItemPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerRotateItemPlugin(ushort openTibiaId, string fileName, LuaTable parameters);
        PlayerRotateItemPlugin GetPlayerRotateItemPlugin(ushort openTibiaId);

        void AddPlayerUseItemPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerUseItemPlugin(ushort openTibiaId, string fileName, LuaTable parameters);
        PlayerUseItemPlugin GetPlayerUseItemPlugin(ushort openTibiaId);

        void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, string fileName, LuaTable parameters);
        PlayerUseItemWithItemPlugin GetPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId);

        void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, string fileName, LuaTable parameters);
        PlayerUseItemWithCreaturePlugin GetPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId);

        void AddPlayerMoveCreaturePlugin(string name, ILuaScope script, LuaTable parameters);
        void AddPlayerMoveCreaturePlugin(string name, string fileName, LuaTable parameters);
        PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(string name);

        void AddPlayerMoveItemPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerMoveItemPlugin(ushort openTibiaId, string fileName, LuaTable parameters);
        PlayerMoveItemPlugin GetPlayerMoveItemPlugin(ushort openTibiaId);

        void AddCreatureStepInPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddCreatureStepInPlugin(ushort openTibiaId, string fileName, LuaTable parameters);
        CreatureStepInPlugin GetCreatureStepInPlugin(ushort openTibiaId);

        void AddCreatureStepOutPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddCreatureStepOutPlugin(ushort openTibiaId, string fileName, LuaTable parameters);
        CreatureStepOutPlugin GetCreatureStepOutPlugin(ushort openTibiaId);

        void AddInventoryEquipPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddInventoryEquipPlugin(ushort openTibiaId, string fileName, LuaTable parameters);
        InventoryEquipPlugin GetInventoryEquipPlugin(ushort openTibiaId);

        void AddInventoryDeEquipPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddInventoryDeEquipPlugin(ushort openTibiaId, string fileName, LuaTable parameters);
        InventoryDeEquipPlugin GetInventoryDeEquipPlugin(ushort openTibiaId);

        void AddPlayerSayPlugin(string message, ILuaScope script, LuaTable parameters);
        void AddPlayerSayPlugin(string message, string fileName, LuaTable parameters);
        PlayerSayPlugin GetPlayerSayPlugin(string message);

        void AddPlayerLoginPlugin(ILuaScope script, LuaTable parameters);
        void AddPlayerLoginPlugin(string fileName, LuaTable parameters);
        IEnumerable<PlayerLoginPlugin> GetPlayerLoginPlugins();

        void AddPlayerLogoutPlugin(ILuaScope script, LuaTable parameters);
        void AddPlayerLogoutPlugin(string fileName, LuaTable parameters);
        IEnumerable<PlayerLogoutPlugin> GetPlayerLogoutPlugins();

        void AddPlayerAdvanceLevelPlugin(string fileName, LuaTable parameters);
        void AddPlayerAdvanceLevelPlugin(ILuaScope script, LuaTable parameters);
        IEnumerable<PlayerAdvanceLevelPlugin> GetPlayerAdvanceLevelPlugins();

        void AddPlayerAdvanceSkillPlugin(string fileName, LuaTable parameters);
        void AddPlayerAdvanceSkillPlugin(ILuaScope script, LuaTable parameters);
        IEnumerable<PlayerAdvanceSkillPlugin> GetPlayerAdvanceSkillPlugins();

        void AddCreatureDeathPlugin(string fileName, LuaTable parameters);
        void AddCreatureDeathPlugin(ILuaScope script, LuaTable parameters);
        IEnumerable<CreatureDeathPlugin> GetCreatureDeathPlugins();

        void AddCreatureKillPlugin(string fileName, LuaTable parameters);
        void AddCreatureKillPlugin(ILuaScope script, LuaTable parameters);
        IEnumerable<CreatureKillPlugin> GetCreatureKillPlugins();

        void AddPlayerEarnAchievementPlugin(string fileName, LuaTable parameters);
        void AddPlayerEarnAchievementPlugin(ILuaScope script, LuaTable parameters);
        IEnumerable<PlayerEarnAchievementPlugin> GetPlayerEarnAchievementPlugins();

        void AddServerStartupPlugin(ILuaScope script, LuaTable parameters);
        void AddServerStartupPlugin(string fileName, LuaTable parameters);
        IEnumerable<ServerStartupPlugin> GetServerStartupPlugins();

        void AddServerShutdownPlugin(ILuaScope script, LuaTable parameters);
        void AddServerShutdownPlugin(string fileName, LuaTable parameters);
        IEnumerable<ServerShutdownPlugin> GetServerShutdownPlugins();

        void AddServerSavePlugin(ILuaScope script, LuaTable parameters);
        void AddServerSavePlugin(string fileName, LuaTable parameters);
        IEnumerable<ServerSavePlugin> GetServerSavePlugins();

        void AddServerRecordPlugin(ILuaScope script, LuaTable parameters);
        void AddServerRecordPlugin(string fileName, LuaTable parameters);
        IEnumerable<ServerRecordPlugin> GetServerRecordPlugins();

        void AddDialoguePlugin(string name, ILuaScope script, LuaTable parameters);
        void AddDialoguePlugin(string name, string fileName, LuaTable parameters);
        DialoguePlugin GetDialoguePlugin(string name);

        void AddItemCreationPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddItemCreationPlugin(ushort openTibiaId, string fileName, LuaTable parameters);
        ItemCreationPlugin GetItemCreationPlugin(ushort openTibiaId);

        void AddMonsterCreationPlugin(string name, ILuaScope script, LuaTable parameters);
        void AddMonsterCreationPlugin(string name, string fileName, LuaTable parameters);
        MonsterCreationPlugin GetMonsterCreationPlugin(string name);

        void AddNpcCreationPlugin(string name, ILuaScope script, LuaTable parameters);
        void AddNpcCreationPlugin(string name, string fileName, LuaTable parameters);
        NpcCreationPlugin GetNpcCreationPlugin(string name);

        void AddPlayerCreationPlugin(string name, ILuaScope script, LuaTable parameters);
        void AddPlayerCreationPlugin(string name, string fileName, LuaTable parameters);
        PlayerCreationPlugin GetPlayerCreationPlugin(string name);

        void AddSpellPlugin(bool requiresTarget, ILuaScope script, LuaTable parameters, Spell spell);
        void AddSpellPlugin(bool requiresTarget, string fileName, LuaTable parameters, Spell spell);
        SpellPlugin GetSpellPlugin(bool requiresTarget, string words);

        void AddRunePlugin(bool requiresTarget, ILuaScope script, LuaTable parameters, Rune rune);
        void AddRunePlugin(bool requiresTarget, string fileName, LuaTable parameters, Rune rune);
        RunePlugin GetRunePlugin(bool requiresTarget, ushort openTibiaId);

        void AddWeaponPlugin(ILuaScope script, LuaTable parameters, Weapon weapon);
        void AddWeaponPlugin(string fileName, LuaTable parameters, Weapon weapon);
        WeaponPlugin GetWeaponPlugin(ushort openTibiaId);

        void AddAmmunitionPlugin(ILuaScope script, LuaTable parameters, Ammunition ammunition);
        void AddAmmunitionPlugin(string fileName, LuaTable parameters, Ammunition ammunition);
        AmmunitionPlugin GetAmmunitionPlugin(ushort openTibiaId);

        void AddRaidPlugin(ILuaScope script, LuaTable parameters, Raid raid);
        void AddRaidPlugin(string fileName, LuaTable parameters, Raid raid);
        RaidPlugin GetRaidPlugin(string name);

        List<Spell> Spells { get; }

        List<Rune> Runes { get; }

        List<Weapon> Weapons { get; }

        List<Ammunition> Ammunitions { get; }

        List<Raid> Raids { get; }

        void Stop();
    }
}