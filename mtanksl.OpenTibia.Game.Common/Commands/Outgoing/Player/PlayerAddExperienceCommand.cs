using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Components;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerAddExperienceCommand : Command
    {
        public PlayerAddExperienceCommand(Player player, ulong experience)
        {
            Player = player;

            Experience = experience;
        }

        public Player Player { get; set; }

        public ulong Experience { get; set; }

        public override async Promise Execute()
        {
            Experience = (ulong)(Experience * Context.Server.Config.GameplayExperienceRate);

            if (Context.Server.Config.GameplayExperienceStages.Enabled)
            {
                foreach (var level in Context.Server.Config.GameplayExperienceStages.Levels)
                {
                    if (Player.Level >= level.MinLevel && Player.Level <= level.MaxLevel)
                    {
                        Experience = (ulong)(Experience * level.Multiplier);

                        break;
                    }
                }
            }

            if (Experience > Player.Level)
            {
                PlayerRegenerationConditionBehaviour playerRegenerationConditionBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerRegenerationConditionBehaviour>(Player);

                if (playerRegenerationConditionBehaviour != null)
                {
                    playerRegenerationConditionBehaviour.AddSoulRegeneration();
                }
            }

            ushort currentLevel = Player.Level;

            ulong currentExperience = Player.Experience;

            ushort correctLevel = currentLevel;

            byte correctLevelPercent = 0;

            ulong minExperience = Formula.GetRequiredExperience(correctLevel);

            while (true)
            {
                ulong maxExperience = Formula.GetRequiredExperience( (ushort)(correctLevel + 1) );

                if (currentExperience + Experience < maxExperience)
                {
                    correctLevelPercent = (byte)Math.Max(0, Math.Min(100, Math.Floor(100.0 * (currentExperience + Experience - minExperience) / (maxExperience - minExperience) ) ) );

                    break;
                }
                else
                {
                    correctLevel++;

                    minExperience = maxExperience;
                }
            }

            await Context.AddCommand(new ShowAnimatedTextCommand(Player, AnimatedTextColor.White, (uint)Experience) );

            if (correctLevel > currentLevel)
            {
                VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)Player.Vocation);

                Player.BaseSpeed = Formula.GetBaseSpeed(correctLevel);

                Player.Capacity = (uint)(Player.Capacity + (correctLevel - currentLevel) * vocationConfig.CapacityPerLevel * 100);

                Player.MaxHealth = (ushort)(Player.MaxHealth + (correctLevel - currentLevel) * vocationConfig.HealthPerLevel);

                Player.Health = (ushort)(Player.Health + (correctLevel - currentLevel) * vocationConfig.HealthPerLevel);

                Player.MaxMana = (ushort)(Player.MaxMana + (correctLevel - currentLevel) * vocationConfig.ManaPerLevel);

                Player.Mana = (ushort)(Player.Mana + (correctLevel - currentLevel) * vocationConfig.ManaPerLevel);

                await Context.AddCommand(new PlayerUpdateExperienceCommand(Player, currentExperience + Experience, correctLevel, correctLevelPercent) );

                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Game, "You advanced from level " + currentLevel + " to level " + correctLevel + ".") );
                
                Context.AddEvent(new PlayerAdvanceLevelEventArgs(Player, currentLevel, correctLevel) );
            }
            else
            {
                await Context.AddCommand(new PlayerUpdateExperienceCommand(Player, currentExperience + Experience, correctLevel, correctLevelPercent) );
            }
        }
    }
}