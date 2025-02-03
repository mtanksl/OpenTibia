﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
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
                currentExperience = Experience;

                correctLevel = 1;

                correctLevelPercent = 0;
            }

            if (correctLevel < currentLevel)
            {
                VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)Player.Vocation);

                Player.BaseSpeed = Formula.GetBaseSpeed(correctLevel);

                uint removeCapacity = (uint)( (correctLevel - currentLevel) * vocationConfig.CapacityPerLevel * 100);

                if (Player.Capacity > removeCapacity)
                {
                    Player.Capacity -= removeCapacity;
                }
                else
                {
                    Player.Capacity = 40000;
                }

                ushort removeHealth = (ushort)( (correctLevel - currentLevel) * vocationConfig.HealthPerLevel);

                if (Player.MaxHealth > removeHealth)
                {
                    Player.MaxHealth -= removeHealth;
                }
                else
                {
                    Player.MaxHealth = 150;
                }

                Player.Health = Math.Min(Player.MaxHealth, Player.Health);

                ushort removeMana = (ushort)( (correctLevel - currentLevel) * vocationConfig.ManaPerLevel);

                if (Player.MaxMana > removeMana)
                {
                    Player.MaxMana -= removeMana;
                }
                else
                {
                    Player.MaxMana = 55;
                }

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