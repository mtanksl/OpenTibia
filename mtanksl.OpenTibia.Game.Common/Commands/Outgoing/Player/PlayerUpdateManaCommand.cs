using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateManaCommand : Command
    {
        public PlayerUpdateManaCommand(Player player, int mana)
        {
            Player = player;

            Mana = (ushort)Math.Max(0, Math.Min(player.MaxMana, mana) );
        }

        public Player Player { get; set; }

        public ushort Mana { get; set; }

        public override Promise Execute()
        {
            if ( !( Player.Rank == Rank.Gamemaster || Player.Rank == Rank.AccountManager || Player.IsDestroyed) )
            {
                if (Player.Mana != Mana)
                {
                    Player.Mana = Mana;

                    Context.AddPacket(Player, new SendStatusOutgoingPacket(
                        Player.Health, Player.MaxHealth, 
                        Player.Capacity, 
                        Player.Experience, Player.Level, Player.LevelPercent, 
                        Player.Mana, Player.MaxMana, 
                        Player.Skills.GetClientSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillPercent(Skill.MagicLevel), 
                        Player.Soul, 
                        Player.Stamina) );

                    Context.AddEvent(new PlayerUpdateManaEventArgs(Player, Mana) );
                }
            }

            return Promise.Completed;
        }
    }
}