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

        void AddPlayerRotateItemPlugin(ushort openTibiaId, PlayerRotateItemPlugin playerRotateItemPlugin);
        void AddPlayerRotateItemPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddPlayerRotateItemPlugin(ushort openTibiaId, string fileName);
        PlayerRotateItemPlugin GetPlayerRotateItemPlugin(ushort openTibiaId);

        void AddPlayerUseItemPlugin(ushort openTibiaId, PlayerUseItemPlugin playerUseItemPlugin);
        void AddPlayerUseItemPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddPlayerUseItemPlugin(ushort openTibiaId, string fileName);
        PlayerUseItemPlugin GetPlayerUseItemPlugin(ushort openTibiaId);

        void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, PlayerUseItemWithItemPlugin playerUseItemWithItemPlugin);
        void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId, string fileName);
        PlayerUseItemWithItemPlugin GetPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId);

        void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, PlayerUseItemWithCreaturePlugin playerUseItemWithCreaturePlugin);
        void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId, string fileName);
        PlayerUseItemWithCreaturePlugin GetPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId);

        void AddPlayerMoveCreaturePlugin(string name, PlayerMoveCreaturePlugin playerMoveCreaturePlugin);
        void AddPlayerMoveCreaturePlugin(string name, LuaScope script, LuaTable parameters);
        void AddPlayerMoveCreaturePlugin(string name, string fileName);
        PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(string name);

        void AddPlayerMoveItemPlugin(ushort openTibiaId, PlayerMoveItemPlugin playerMoveItemPlugin);
        void AddPlayerMoveItemPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddPlayerMoveItemPlugin(ushort openTibiaId, string fileName);
        PlayerMoveItemPlugin GetPlayerMoveItemPlugin(ushort openTibiaId);

        void AddCreatureStepInPlugin(ushort openTibiaId, CreatureStepInPlugin creatureStepInPlugin);
        void AddCreatureStepInPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddCreatureStepInPlugin(ushort openTibiaId, string fileName);
        CreatureStepInPlugin GetCreatureStepInPlugin(ushort openTibiaId);

        void AddCreatureStepOutPlugin(ushort openTibiaId, CreatureStepOutPlugin creatureStepOutPlugin);
        void AddCreatureStepOutPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddCreatureStepOutPlugin(ushort openTibiaId, string fileName);
        CreatureStepOutPlugin GetCreatureStepOutPlugin(ushort openTibiaId);

        void AddInventoryEquipPlugin(ushort openTibiaId, InventoryEquipPlugin inventoryEquipPlugin);
        void AddInventoryEquipPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddInventoryEquipPlugin(ushort openTibiaId, string fileName);
        InventoryEquipPlugin GetInventoryEquipPlugin(ushort openTibiaId);

        void AddInventoryDeEquipPlugin(ushort openTibiaId, InventoryDeEquipPlugin inventoryDeEquipPlugin);
        void AddInventoryDeEquipPlugin(ushort openTibiaId, LuaScope script, LuaTable parameters);
        void AddInventoryDeEquipPlugin(ushort openTibiaId, string fileName);
        InventoryDeEquipPlugin GetInventoryDeEquipPlugin(ushort openTibiaId);

        void AddPlayerSayPlugin(string message, PlayerSayPlugin playerSayPlugin);
        void AddPlayerSayPlugin(string message, LuaScope script, LuaTable parameters);
        void AddPlayerSayPlugin(string message, string fileName);
        PlayerSayPlugin GetPlayerSayPlugin(string message);

        void AddPlayerLoginPlugin(PlayerLoginPlugin playerLoginPlugin);
        void AddPlayerLoginPlugin(LuaScope script, LuaTable parameters);
        void AddPlayerLoginPlugin(string fileName);
        IEnumerable<PlayerLoginPlugin> GetPlayerLoginPlugins();

        void AddPlayerLogoutPlugin(PlayerLogoutPlugin playerLogoutPlugin);
        void AddPlayerLogoutPlugin(LuaScope script, LuaTable parameters);
        void AddPlayerLogoutPlugin(string fileName);
        IEnumerable<PlayerLogoutPlugin> GetPlayerLogoutPlugins();

        void AddDialoguePlugin(string name, Func<DialoguePlugin> dialoguePlugin);
        void AddDialoguePlugin(string name, LuaScope script, LuaTable parameters);
        void AddDialoguePlugin(string name, string fileName);
        DialoguePlugin GetDialoguePlugin(string name);

        void AddSpellPlugin(bool requiresTarget, SpellPlugin spellPlugin);
        void AddSpellPlugin(bool requiresTarget, LuaScope script, LuaTable parameters, Spell spell);
        void AddSpellPlugin(bool requiresTarget, string fileName, Spell spell);
        SpellPlugin GetSpellPlugin(bool requiresTarget, string words);

        void AddRunePlugin(bool requiresTarget, RunePlugin runePlugin);
        void AddRunePlugin(bool requiresTarget, LuaScope script, LuaTable parameters, Rune rune);
        void AddRunePlugin(bool requiresTarget, string fileName, Rune rune);
        RunePlugin GetRunePlugin(bool requiresTarget, ushort openTibiaId);

        void AddWeaponPlugin(WeaponPlugin weaponPlugin);
        void AddWeaponPlugin(LuaScope script, LuaTable parameters, Weapon weapon);
        void AddWeaponPlugin(string fileName, Weapon weapon);
        WeaponPlugin GetWeaponPlugin(ushort openTibiaId);

        void AddAmmunitionPlugin(AmmunitionPlugin ammunitionPlugin);
        void AddAmmunitionPlugin(LuaScope script, LuaTable parameters, Ammunition ammunition);
        void AddAmmunitionPlugin(string fileName, Ammunition ammunition);
        AmmunitionPlugin GetAmmunitionPlugin(ushort openTibiaId);

        List<Spell> Spells { get; }

        List<Rune> Runes { get; }

        List<Weapon> Weapons { get; }

        List<Ammunition> Ammunitions { get; }

        void Stop();
    }
}