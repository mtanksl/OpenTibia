using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CleanUpPartyCollectionHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature is Player player)
            {
                return next().Then( () =>
                {
                    Party party = Context.Server.Parties.GetPartyThatContainsMember(player);

                    if (party != null)
                    {
                        foreach (var member in party.GetMembers() )
                        {
                            Context.AddPacket(player, new SetPartyIconOutgoingPacket(member.Id, PartyIcon.None) );

                            if (member != player)
                            {
                                Context.AddPacket(member, new SetPartyIconOutgoingPacket(player.Id, PartyIcon.None) );
                            }
                        }

                        party.RemoveMember(player);

                        if (player == party.Leader)
                        {
                            if (party.CountMembers == 0)
                            {
                                foreach (var invitation in party.GetInvitations() )
                                {
                                    Context.AddPacket(player, new SetPartyIconOutgoingPacket(invitation.Id, PartyIcon.None) );

                                    Context.AddPacket(invitation, new SetPartyIconOutgoingPacket(player.Id, PartyIcon.None) );
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
                                    Context.AddPacket(player, new SetPartyIconOutgoingPacket(invitation.Id, PartyIcon.None) );

                                    Context.AddPacket(invitation, new SetPartyIconOutgoingPacket(player.Id, PartyIcon.None) );

                                    Context.AddPacket(party.Leader, new SetPartyIconOutgoingPacket(invitation.Id, PartyIcon.WhiteBlue) );

                                    Context.AddPacket(invitation, new SetPartyIconOutgoingPacket(party.Leader.Id, PartyIcon.WhiteYellow) );
                                }
                            }
                        }      
                    }

                    foreach (var party2 in Context.Server.Parties.GetPartyThatContainsInvitation(player) )
                    {
                        party2.RemoveInvitation(player);

                        Context.AddPacket(party2.Leader, new SetPartyIconOutgoingPacket(player.Id, PartyIcon.None) );

                        Context.AddPacket(player, new SetPartyIconOutgoingPacket(party2.Leader.Id, PartyIcon.None) );
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}