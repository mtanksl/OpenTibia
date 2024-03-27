using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseInviteToPartyCommand : Command
    {
        public ParseInviteToPartyCommand(Player player, uint creatureId)
        {
            Player = player;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public override Promise Execute()
        {
            Party party = Context.Server.Parties.GetPartyThatContainsMember(Player);

            if (party == null || party.Leader == Player)
            {
                Player observer = Context.Server.GameObjects.GetPlayer(CreatureId);

                if (observer != null && observer != Player)
                {
                    Party observerParty = Context.Server.Parties.GetPartyThatContainsMember(observer);

                    if (observerParty != null)
                    {
                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " is already in a party.") );

                        return Promise.Break;
                    }

                    if (party == null)
                    {
                        party = new Party()
                        {
                            Leader = Player,

                            SharedExperienceEnabled = false
                        };

                        party.AddMember(Player);

                        foreach (var party2 in Context.Server.Parties.GetPartyThatContainsInvitation(Player) )
                        {
                            party2.RemoveInvitation(Player);

                            Context.AddPacket(party2.Leader, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.None) );

                            Context.AddPacket(Player, new SetPartyIconOutgoingPacket(party2.Leader.Id, PartyIcon.None) );
                        }

                        Context.Server.Parties.AddParty(party);

                        Context.AddPacket(Player, new SetPartyIconOutgoingPacket(Player.Id, party.SharedExperienceEnabled ? PartyIcon.YellowSharedExperience : PartyIcon.Yellow) ); // Leader
                    }

                    party.AddInvitation(observer);

                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited.") );
                                   
                    Context.AddPacket(Player, new SetPartyIconOutgoingPacket(observer.Id, PartyIcon.WhiteBlue) ); // Invitee

                    Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " invites you to " + (Player.Gender == Gender.Male ? "his" : "her") + " party." ) );

                    Context.AddPacket(observer, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.WhiteYellow) ); // Inviter
                }
            }

            return Promise.Completed;
        }
    }
}