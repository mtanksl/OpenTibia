using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateExperienceCommand : Command
    {
        public PlayerUpdateExperienceCommand(Player player, ulong experience, ushort level, byte levelPercent)
        {
            Player = player;

            Experience = experience;

            Level = level;

            LevelPercent = levelPercent;
        }

        public Player Player { get; set; }

        public ulong Experience { get; set; }

        public ushort Level { get; set; }

        public byte LevelPercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Experience != Experience || Player.Level != Level || Player.LevelPercent != LevelPercent)
            {
                Player.Experience = Experience;

                Player.Level = Level;

                Player.LevelPercent = LevelPercent;

                Context.AddPacket(Player, new SendStatusOutgoingPacket(
                        Player.Health, Player.MaxHealth, 
                        Player.Capacity, 
                        Player.Experience, Player.Level, Player.LevelPercent, 
                        Player.Mana, Player.MaxMana, 
                        Player.Skills.GetClientSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillPercent(Skill.MagicLevel), 
                        Player.Soul, 
                        Player.Stamina) );

                Context.AddEvent(new PlayerUpdateExperienceEventArgs(Player, Experience, Level) );
            }

            return Promise.Completed;
        }
    }
}