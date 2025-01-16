using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateAxeCommand : Command
    {
        public PlayerUpdateAxeCommand(Player player, ulong axePoints, byte axe, byte axePercent)
        {
            Player = player;

            AxePoints = axePoints;

            Axe = axe;

            AxePercent = axePercent;
        }

        public Player Player { get; set; }

        public ulong AxePoints { get; set; }

        public byte Axe { get; set; }

        public byte AxePercent { get; set; }

        public override Promise Execute()
        {
            if (Player.Skills.AxePoints != AxePoints || Player.Skills.Axe != Axe || Player.Skills.AxePercent != AxePercent)
            {
                Player.Skills.AxePoints = AxePoints;

                if (Player.Skills.Axe != Axe || Player.Skills.AxePercent != AxePercent)
                {
                    Player.Skills.Axe = Axe;

                    Player.Skills.AxePercent = AxePercent;

                    Context.AddPacket(Player, new SendSkillsOutgoingPacket(Player.Skills.Fist, Player.Skills.FistPercent, Player.Skills.Club, Player.Skills.ClubPercent, Player.Skills.Sword, Player.Skills.SwordPercent, Player.Skills.Axe, Player.Skills.AxePercent, Player.Skills.Distance, Player.Skills.DistancePercent, Player.Skills.Shield, Player.Skills.ShieldPercent, Player.Skills.Fish, Player.Skills.FishPercent) );
                }

                Context.AddEvent(new PlayerUpdateAxeEventArgs(Player, AxePoints, Axe) );
            }

            return Promise.Completed;
        }
    }
}