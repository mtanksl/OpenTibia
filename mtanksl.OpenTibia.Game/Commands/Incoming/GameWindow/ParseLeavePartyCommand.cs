using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseLeavePartyCommand : IncomingCommand
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
                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You have left the party.") );

                foreach (var member in party.GetMembers() )
                {
                    Context.AddPacket(Player, new SetPartyIconOutgoingPacket(member.Id, PartyIcon.None) );

                    if (member != Player)
                    {
                        Context.AddPacket(member, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " has left the party.") );

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
                            Context.AddPacket(invitation, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " has revoked " + (Player.Gender == Gender.Male ? "his" : "her") + " invitation.") );

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
                            if (member == party.Leader)
                            {
                                Context.AddPacket(member, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You are now the leader of the party.") );
                            }
                            else
                            {
                                Context.AddPacket(member, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, party.Leader.Name + " is now the leader of the party.") );
                            }

                            Context.AddPacket(member, new SetPartyIconOutgoingPacket(party.Leader.Id, party.SharedExperienceEnabled ? PartyIcon.YellowSharedExperience : PartyIcon.Yellow) );
                        }

                        foreach (var invitation in party.GetInvitations() )
                        {
                            Context.AddPacket(invitation, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, party.Leader.Name + " is now the leader of the party.") );

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