﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerLoginCommand : Command
    {
        public PlayerLoginCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            Context.AddPacket(Player, new SendInfoOutgoingPacket(Player.Id, Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster) );

            Context.AddPacket(Player, new SendTilesOutgoingPacket(Context.Server.Map, Player.Client, Player.Tile.Position) );

            Context.AddPacket(Player, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );
                                
            Context.AddPacket(Player, new SendStatusOutgoingPacket(
                    Player.Health, Player.MaxHealth, 
                    Player.Capacity, 
                    Player.Experience, Player.Level, Player.LevelPercent, 
                    Player.Mana, Player.MaxMana, 
                    Player.Skills.GetClientSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillPercent(Skill.MagicLevel), 
                    Player.Soul, 
                    Player.Stamina) );

            Context.AddPacket(Player, new SendSkillsOutgoingPacket(
                Player.Skills.GetClientSkillLevel(Skill.Fist), Player.Skills.GetSkillPercent(Skill.Fist),
                Player.Skills.GetClientSkillLevel(Skill.Club), Player.Skills.GetSkillPercent(Skill.Club),
                Player.Skills.GetClientSkillLevel(Skill.Sword), Player.Skills.GetSkillPercent(Skill.Sword),
                Player.Skills.GetClientSkillLevel(Skill.Axe), Player.Skills.GetSkillPercent(Skill.Axe),
                Player.Skills.GetClientSkillLevel(Skill.Distance), Player.Skills.GetSkillPercent(Skill.Distance),
                Player.Skills.GetClientSkillLevel(Skill.Shield), Player.Skills.GetSkillPercent(Skill.Shield),
                Player.Skills.GetClientSkillLevel(Skill.Fish), Player.Skills.GetSkillPercent(Skill.Fish) ) );

            Context.AddPacket(Player, new SetSpecialConditionOutgoingPacket(Player.SpecialConditions) );

            foreach (var pair in Player.Inventory.GetIndexedContents() )
            {
                Context.AddPacket(Player, new SlotAddOutgoingPacket( (byte)pair.Key, (Item)pair.Value) );
            }

            foreach (var pair in Player.Vips.GetIndexed() )
            {
                Context.AddPacket(Player, new VipOutgoingPacket( (uint)pair.Key, pair.Value, Context.Server.GameObjects.GetPlayerByName(pair.Value) != null) );
            }

            Context.AddEvent(new PlayerLoginEventArgs(Player) );

            return Promise.Completed;
        }
    }
}