using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseJoinPartyCommand : Command
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

                            Context.AddPacket(party2.Leader, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.None) );

                            Context.AddPacket(Player, new SetPartyIconOutgoingPacket(party2.Leader.Id, PartyIcon.None) );
                        }

                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " has joined the party.") );

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