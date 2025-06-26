using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseJoinPartyCommand : IncomingCommand
    {
        public ParseJoinPartyCommand(Player player, uint creatureId)
        {
            Player = player;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public override Promise Execute()
        {
            Party party = Context.Server.Parties.GetPartyThatContainsMember(Player);

            if (party == null)
            {
                Player observer = Context.Server.GameObjects.GetPlayer(CreatureId);

                if (observer != null && observer != Player)
                {
                    Party observerParty = Context.Server.Parties.GetPartyByLeader(observer);

                    if (observerParty != null && observerParty.ContainsInvitation(Player) )
                    {                                                   
                        observerParty.RemoveInvitation(Player);

                        observerParty.AddMember(Player);

                        foreach (var party2 in Context.Server.Parties.GetPartyThatContainsInvitation(Player) )
                        {
                            party2.RemoveInvitation(Player);
                          
                            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "You have revoked " + party2.Leader.Name + "'s invitation.") );

                            Context.AddPacket(Player, new SetPartyIconOutgoingPacket(party2.Leader.Id, PartyIcon.None) );

                            Context.AddPacket(party2.Leader, new ShowWindowTextOutgoingPacket(MessageMode.Look, Player.Name + " has revoked your invitation.") );

                            Context.AddPacket(party2.Leader, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.None) );    
                        }

                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "You have joined " + observerParty.Leader.Name + "'s party. Open the party channel to communicate with your companions.") );

                        foreach (var member in observerParty.GetMembers() )
                        {
                            if (member == observerParty.Leader)
                            {
                                Context.AddPacket(Player, new SetPartyIconOutgoingPacket(member.Id, observerParty.SharedExperienceEnabled ? PartyIcon.YellowSharedExperience : PartyIcon.Yellow) );
                            }
                            else
                            {
                                Context.AddPacket(Player, new SetPartyIconOutgoingPacket(member.Id, observerParty.SharedExperienceEnabled ? PartyIcon.BlueSharedExperience : PartyIcon.Blue) );
                            }

                            if (member != Player)
                            {
                                Context.AddPacket(member, new ShowWindowTextOutgoingPacket(MessageMode.Look, Player.Name + " has joined the party.") );

                                Context.AddPacket(member, new SetPartyIconOutgoingPacket(Player.Id, observerParty.SharedExperienceEnabled ? PartyIcon.BlueSharedExperience : PartyIcon.Blue) );
                            }
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}