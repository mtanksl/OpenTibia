using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPluginCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        PlayerRotateItemPlugin GetPlayerRotateItemPlugin(ushort openTibiaId);

        PlayerUseItemPlugin GetPlayerUseItemPlugin(ushort openTibiaId);

        PlayerUseItemWithItemPlugin GetPlayerUseItemWithItemPlugin(bool allowFarUse, ushort openTibiaId);

        PlayerUseItemWithCreaturePlugin GetPlayerUseItemWithCreaturePlugin(bool allowFarUse, ushort openTibiaId);

        PlayerMoveCreaturePlugin GetPlayerMoveCreaturePlugin(string name);

        PlayerMoveItemPlugin GetPlayerMoveItemPlugin(ushort openTibiaId);

        CreatureStepInPlugin GetCreatureStepInPlugin(ushort openTibiaId);

        CreatureStepOutPlugin GetCreatureStepOutPlugin(ushort openTibiaId);

        InventoryEquipPlugin GetInventoryEquipPlugin(ushort openTibiaId);

        InventoryDeEquipPlugin GetInventoryDeEquipPlugin(ushort openTibiaId);

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