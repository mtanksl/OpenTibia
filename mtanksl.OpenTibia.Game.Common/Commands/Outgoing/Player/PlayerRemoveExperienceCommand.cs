using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerRemoveExperienceCommand : Command
    {
        public PlayerRemoveExperienceCommand(Player player, ulong experience)
        {
            Player = player;

            Experience = experience;
        }

        public Player Player { get; set; }

        public ulong Experience { get; set; }

        public override async Promise Execute()
        {
            ushort currentLevel = Player.Level;

            ulong currentExperience = Player.Experience;

            ushort correctLevel;

            byte correctLevelPercent;

            if (currentExperience > Experience)
            {
                correctLevel = currentLevel;

                correctLevelPercent = 0;

                ulong maxExperience = Formula.GetRequiredExperience( (ushort)(correctLevel + 1) );

                while (true)
                {
                    ulong minExperience = Formula.GetRequiredExperience(correctLevel);

                    if (currentExperience - Experience >= minExperience)
                    {
                        correctLevelPercent = (byte)Math.Max(0, Math.Min(100, Math.Floor(100.0 * (currentExperience - Experience - minExperience) / (maxExperience - minExperience) ) ) );

                        break;
                    }
                    else
                    {
                        correctLevel--;

                        maxExperience = minExperience;
                    }
                }                            
            }
            else
            {
                correctLevel = 1;

                correctLevelPercent = 0;
            }

            if (correctLevel < currentLevel)
            {
                VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById((byte)Player.Vocation);

                Player.BaseSpeed = Formula.GetBaseSpeed(correctLevel);

                Player.Capacity = (uint)(Player.Capacity - (correctLevel - currentLevel) * vocationConfig.CapacityPerLevel * 100);

                Player.MaxHealth = (ushort)(Player.MaxHealth - (correctLevel - currentLevel) * vocationConfig.HealthPerLevel);

                Player.Health = Math.Min(Player.MaxHealth, Player.Health);

                Player.MaxMana = (ushort)(Player.MaxMana - (correctLevel - currentLevel) * vocationConfig.ManaPerLevel);

                Player.Mana = Math.Min(Player.MaxMana, Player.Mana);

                await Context.AddCommand(new PlayerUpdateExperienceCommand(Player, currentExperience - Experience, correctLevel, correctLevelPercent) );

                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You downgraded from level " + currentLevel + " to level " + correctLevel + ".") );
                  
                Context.AddEvent(Player, new PlayerAdvanceLevelEventArgs(Player, currentLevel, correctLevel) );
            }
            else
            {
                await Context.AddCommand(new PlayerUpdateExperienceCommand(Player, currentExperience - Experience, correctLevel, correctLevelPercent) );
            }
        }
    }
}