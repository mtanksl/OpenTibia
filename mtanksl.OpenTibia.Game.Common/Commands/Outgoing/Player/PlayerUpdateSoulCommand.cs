using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateSoulCommand : Command
    {
        public PlayerUpdateSoulCommand(Player player, int soul) : this(player, soul, 200)
        {

        }

        public PlayerUpdateSoulCommand(Player player, int soul, int maxSoul)
        {
            Player = player;

            Soul = (byte)Math.Max(0, Math.Min(maxSoul, soul) );
        }

        public Player Player { get; set; }

        public byte Soul { get; set; }

        public override Promise Execute()
        {
            if (Player.Rank != Rank.Gamemaster && Player.Rank != Rank.AccountManager)
            {
                if (Player.Soul != Soul)
                {
                    Player.Soul = Soul;

                    Context.AddPacket(Player, new SendStatusOutgoingPacket(
                        Player.Health, Player.MaxHealth, 
                        Player.Capacity, 
                        Player.Experience, Player.Level, Player.LevelPercent, 
                        Player.Mana, Player.MaxMana, 
                        Player.Skills.GetSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillPercent(Skill.MagicLevel), 
                        Player.Soul, 
                        Player.Stamina) );

                    Context.AddEvent(new PlayerUpdateSoulEventArgs(Player, Soul) );
                }
            }

            return Promise.Completed;
        }
    }
}