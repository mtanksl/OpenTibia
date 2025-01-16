using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
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
            // VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)Player.Vocation);

            ushort level = Player.Level;

            ulong experience = Player.Experience;


            ushort correctLevel = level;

            byte correctLevelPercent = 0;

            ulong minExperience = Formula.GetRequiredExperience(level);

            while (true)
            {
                ulong maxExperience = Formula.GetRequiredExperience( (ushort)(level + 1) );

                if (experience + Experience < maxExperience)
                {
                    correctLevelPercent = (byte)Math.Ceiling(100.0 * (experience - minExperience) / (maxExperience - minExperience) );

                    break;
                }
                else
                {
                    level++;

                    minExperience = maxExperience;
                }
            }

            await Context.AddCommand(new ShowAnimatedTextCommand(Player, AnimatedTextColor.White, Experience.ToString() ) );

            if (correctLevel > level)
            {
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You advanced from level " + level + " to level " + correctLevel + ".") );
            }

            await Context.AddCommand(new PlayerUpdateExperienceCommand(Player, experience + Experience, correctLevel, correctLevelPercent) );
        }
    }
}