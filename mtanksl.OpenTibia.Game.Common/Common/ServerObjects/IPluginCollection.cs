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
        void AddPlayerRotateItemPlugin(ushort openTibiaId, string fileName);
        PlayerRotateItemPlugin GetPlayerRotateItemPlugin(ushort openTibiaId);

        void AddPlayerUseItemPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerUseItemPlugin(ushort openTibiaId, string fileName);
        PlayerUseItemPlugin GetPlayerUseItemPlugin(ushort openTibiaId);

        void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, string fileName);
        PlayerUseItemWithItemPlugin GetPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId);

        void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, string fileName);
        PlayerUseItemWithCreaturePlugin GetPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId);

        void AddPlayerMoveCreaturePlugin(string name, ILuaScope script, LuaTable parameters);
        void AddPlayerMoveCreaturePlugin(string name, string fileName);
        PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(string name);

        void AddPlayerMoveItemPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddPlayerMoveItemPlugin(ushort openTibiaId, string fileName);
        PlayerMoveItemPlugin GetPlayerMoveItemPlugin(ushort openTibiaId);

        void AddCreatureStepInPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddCreatureStepInPlugin(ushort openTibiaId, string fileName);
        CreatureStepInPlugin GetCreatureStepInPlugin(ushort openTibiaId);

        void AddCreatureStepOutPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddCreatureStepOutPlugin(ushort openTibiaId, string fileName);
        CreatureStepOutPlugin GetCreatureStepOutPlugin(ushort openTibiaId);

        void AddInventoryEquipPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddInventoryEquipPlugin(ushort openTibiaId, string fileName);
        InventoryEquipPlugin GetInventoryEquipPlugin(ushort openTibiaId);

        void AddInventoryDeEquipPlugin(ushort openTibiaId, ILuaScope script, LuaTable parameters);
        void AddInventoryDeEquipPlugin(ushort openTibiaId, string fileName);
        InventoryDeEquipPlugin GetInventoryDeEquipPlugin(ushort openTibiaId);

        void AddPlayerSayPlugin(string message, ILuaScope script, LuaTable parameters);
        void AddPlayerSayPlugin(string message, string fileName);
        PlayerSayPlugin GetPlayerSayPlugin(string message);

        void AddPlayerLoginPlugin(ILuaScope script, LuaTable parameters);
        void AddPlayerLoginPlugin(string fileName);
        IEnumerable<PlayerLoginPlugin> GetPlayerLoginPlugins();

        void AddPlayerLogoutPlugin(ILuaScope script, LuaTable parameters);
        void AddPlayerLogoutPlugin(string fileName);
        IEnumerable<PlayerLogoutPlugin> GetPlayerLogoutPlugins();

        void AddPlayerAdvanceLevelPlugin(string fileName);
        void AddPlayerAdvanceLevelPlugin(ILuaScope script, LuaTable parameters);
        IEnumerable<PlayerAdvanceLevelPlugin> GetPlayerAdvanceLevelPlugins();

        void AddPlayerAdvanceSkillPlugin(string fileName);
        void AddPlayerAdvanceSkillPlugin(ILuaScope script, LuaTable parameters);
        IEnumerable<PlayerAdvanceSkillPlugin> GetPlayerAdvanceSkillPlugins();

        void AddServerStartupPlugin(ILuaScope script, LuaTable parameters);
        void AddServerStartupPlugin(string fileName);
        IEnumerable<ServerStartupPlugin> GetServerStartupPlugins();

        void AddServerShutdownPlugin(ILuaScope script, LuaTable parameters);
        void AddServerShutdownPlugin(string fileName);
        IEnumerable<ServerShutdownPlugin> GetServerShutdownPlugins();

        void AddServerSavePlugin(ILuaScope script, LuaTable parameters);
        void AddServerSavePlugin(string fileName);
        IEnumerable<ServerSavePlugin> GetServerSavePlugins();

        void AddServerRecordPlugin(ILuaScope script, LuaTable parameters);
        void AddServerRecordPlugin(string fileName);
        IEnumerable<ServerRecordPlugin> GetServerRecordPlugins();

        void AddDialoguePlugin(string name, ILuaScope script, LuaTable parameters);
        void AddDialoguePlugin(string name, string fileName);
        DialoguePlugin GetDialoguePlugin(string name);

        void AddSpellPlugin(bool requiresTarget, ILuaScope script, LuaTable parameters, Spell spell);
        void AddSpellPlugin(bool requiresTarget, string fileName, Spell spell);
        SpellPlugin GetSpellPlugin(bool requiresTarget, string words);

        void AddRunePlugin(bool requiresTarget, ILuaScope script, LuaTable parameters, Rune rune);
        void AddRunePlugin(bool requiresTarget, string fileName, Rune rune);
        RunePlugin GetRunePlugin(bool requiresTarget, ushort openTibiaId);

        void AddWeaponPlugin(ILuaScope script, LuaTable parameters, Weapon weapon);
        void AddWeaponPlugin(string fileName, Weapon weapon);
        WeaponPlugin GetWeaponPlugin(ushort openTibiaId);

        void AddAmmunitionPlugin(ILuaScope script, LuaTable parameters, Ammunition ammunition);
        void AddAmmunitionPlugin(string fileName, Ammunition ammunition);
        AmmunitionPlugin GetAmmunitionPlugin(ushort openTibiaId);

        void AddRaidPlugin(ILuaScope script, LuaTable parameters, Raid raid);
        void AddRaidPlugin(string fileName, Raid raid);
        RaidPlugin GetRaidPlugin(string name);

        List<Spell> Spells { get; }

        List<Rune> Runes { get; }

        List<Weapon> Weapons { get; }

        List<Ammunition> Ammunitions { get; }

        List<Raid> Raids { get; }

        void Stop();
    }
}