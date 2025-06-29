using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

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
            Context.AddPacket(Player, new SendInfoOutgoingPacket(Player.Id, Constants.ServerBeat, Constants.CreatureSpeedA, Constants.CreatureSpeedB, Constants.CreatureSpeedC, Player.Rank == Rank.Tutor || Player.Rank == Rank.Gamemaster) );

            if (Context.Server.Features.HasFeatureFlag(FeatureFlag.LoginPending) )
            {
                Context.AddPacket(Player, new SendPendingStateOutgoingPacket() );

                Context.AddPacket(Player, new SendEnterWorldOutgoingPacket() );
            }
            
            Context.AddPacket(Player, new SendTilesOutgoingPacket(Context.Server.Map, Player.Client, Player.Tile.Position) );
            
            Context.AddPacket(Player, new SetEnvironmentLightOutgoingPacket(Context.Server.Clock.Light) );
                                
            Context.AddPacket(Player, new SendStatusOutgoingPacket(
                    Player.Health, Player.MaxHealth, 
                    Player.Capacity, Player.MaxCapacity,
                    Player.Experience, Player.Level, Player.LevelPercent, 
                    Player.Mana, Player.MaxMana, 
                    Player.Skills.GetClientSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillPercent(Skill.MagicLevel), 
                    Player.Soul, 
                    Player.Stamina,
                    Player.BaseSpeed) );
            
            Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills) );
            
            Context.AddPacket(Player, new SetSpecialConditionOutgoingPacket(Player.SpecialConditions) );
            
            foreach (var pair in Player.Inventory.GetIndexedContents() )
            {
                Context.AddPacket(Player, new SlotAddOutgoingPacket( (byte)pair.Key, (Item)pair.Value) );
            }
            
            foreach (var pair in Player.Vips.GetIndexed() )
            {
                var vip = pair.Value;
            
                Context.AddPacket(Player, new VipOutgoingPacket( (uint)pair.Key, vip.Name, vip.Description, vip.IconId, vip.NotifyLogin, Context.Server.GameObjects.GetPlayerByName(vip.Name) != null) );
            }
            
            if (Context.Server.Features.HasFeatureFlag(FeatureFlag.PlayerBasicData) )
            {
                uint premiumDays = Player.PremiumUntil != null ? Math.Max(0, Math.Min(ushort.MaxValue, (uint)Math.Ceiling( (Player.PremiumUntil.Value - DateTime.UtcNow).TotalDays) ) ) : 0;
            
                Context.AddPacket(Player, new SendBasicDataOutgoingPacket(Player.Premium, premiumDays, Player.Vocation, new List<int>() ) ); //TODO: FeatureFlag.PlayerBasicData
            }

            Context.AddEvent(new PlayerLoginEventArgs(Player) );

            return Promise.Completed;
        }
    }
}