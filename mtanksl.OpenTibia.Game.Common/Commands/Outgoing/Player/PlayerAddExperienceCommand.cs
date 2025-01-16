using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
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
            VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)Player.Vocation);

            ushort level = Player.Level;

            ulong experience = Player.Experience;


            ushort correctLevel = level;

            byte correctLevelPercent = 0;

            ulong minExperience = Formula.GetRequiredExperience(correctLevel);

            while (true)
            {
                ulong maxExperience = Formula.GetRequiredExperience( (ushort)(correctLevel + 1) );

                if (experience + Experience < maxExperience)
                {
                    correctLevelPercent = (byte)Math.Ceiling(100.0 * (experience + Experience - minExperience) / (maxExperience - minExperience) );

                    break;
                }
                else
                {
                    correctLevel++;

                    minExperience = maxExperience;
                }
            }

            await Context.AddCommand(new ShowAnimatedTextCommand(Player, AnimatedTextColor.White, Experience.ToString() ) );

            if (correctLevel > level)
            {
                Player.Capacity = (uint)(Player.Capacity + (correctLevel - level) * vocationConfig.CapacityPerLevel * 100);

                Player.Health = (ushort)(Player.Health + (correctLevel - level) * vocationConfig.HealthPerLevel);

                Player.MaxHealth = (ushort)(Player.MaxHealth + (correctLevel - level) * vocationConfig.HealthPerLevel);

                Player.Mana = (ushort)(Player.Mana + (correctLevel - level) * vocationConfig.ManaPerLevel);

                Player.MaxMana = (ushort)(Player.MaxMana + (correctLevel - level) * vocationConfig.ManaPerLevel);

                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You advanced from level " + level + " to level " + correctLevel + ".") );
                
                Context.AddEvent(new PlayerAdvanceLevelEventArgs(Player, level, correctLevel) );
            }

            await Context.AddCommand(new PlayerUpdateExperienceCommand(Player, experience + Experience, correctLevel, correctLevelPercent) );
        }
    }
}