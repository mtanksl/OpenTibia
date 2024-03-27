using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseLeavePartyCommand : Command
    {
        public ParseLeavePartyCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            Party party = Context.Server.Parties.GetPartyThatContainsMember(Player);

            if (party != null)
            {
                foreach (var member in party.GetMembers() )
                {
                    Context.AddPacket(Player, new SetPartyIconOutgoingPacket(member.Id, PartyIcon.None) );

                    if (member != Player)
                    {
                        Context.AddPacket(member, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.None) );
                    }
                }

                party.RemoveMember(Player);

                if (Player == party.Leader)
                {
                    if (party.CountMembers == 0)
                    {
                        foreach (var invitation in party.GetInvitations() )
                        {
                            Context.AddPacket(Player, new SetPartyIconOutgoingPacket(invitation.Id, PartyIcon.None) );

                            Context.AddPacket(invitation, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.None) );
                        }

                        Context.Server.Parties.RemoveParty(party);
                    }
                    else
                    {
                        party.Leader = party.NextMember();

                        foreach (var member in party.GetMembers() )
                        {
                            Context.AddPacket(member, new SetPartyIconOutgoingPacket(party.Leader.Id, party.SharedExperienceEnabled ? PartyIcon.YellowSharedExperience : PartyIcon.Yellow) );
                        }

                        foreach (var invitation in party.GetInvitations() )
                        {
                            Context.AddPacket(Player, new SetPartyIconOutgoingPacket(invitation.Id, PartyIcon.None) );

                            Context.AddPacket(invitation, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.None) );

                            Context.AddPacket(party.Leader, new SetPartyIconOutgoingPacket(invitation.Id, PartyIcon.WhiteBlue) );

                            Context.AddPacket(invitation, new SetPartyIconOutgoingPacket(party.Leader.Id, PartyIcon.WhiteYellow) );
                        }
                    }
                }      
            }

            return Promise.Completed;
        }
    }
}