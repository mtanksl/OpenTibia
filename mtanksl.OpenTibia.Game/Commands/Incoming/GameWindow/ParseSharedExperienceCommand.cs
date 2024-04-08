using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseSharedExperienceCommand : IncomingCommand
    {
        public ParseSharedExperienceCommand(Player player, bool enabled)
        {
            Player = player;

            Enabled = enabled;
        }

        public Player Player { get; set; }

        public bool Enabled { get; set; }

        public override Promise Execute()
        {
            Party party = Context.Server.Parties.GetPartyByLeader(Player);

            if (party != null)
            {
                party.SharedExperienceEnabled = Enabled;

                foreach (var member in party.GetMembers() )
                {
                    foreach (var member2 in party.GetMembers() )
                    {
                        if (member2 == party.Leader)
                        {
                            Context.AddPacket(member, new SetPartyIconOutgoingPacket(member2.Id, party.SharedExperienceEnabled ? PartyIcon.YellowSharedExperience : PartyIcon.Yellow) );
                        }
                        else
                        {
                            Context.AddPacket(member, new SetPartyIconOutgoingPacket(member2.Id, party.SharedExperienceEnabled ? PartyIcon.BlueSharedExperience : PartyIcon.Blue) );
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}