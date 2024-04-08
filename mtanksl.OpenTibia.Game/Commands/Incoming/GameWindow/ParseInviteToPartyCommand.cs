using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseInviteToPartyCommand : IncomingCommand
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

                    bool createParty = party == null;

                    if (createParty)
                    {
                        party = new Party()
                        {
                            Leader = Player,

                            SharedExperienceEnabled = false
                        };

                        Context.Server.Parties.AddParty(party);

                        foreach (var party2 in Context.Server.Parties.GetPartyThatContainsInvitation(Player) )
                        {
                            party2.RemoveInvitation(Player);

                            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You have revoked " + party2.Leader.Name + "'s invitation.") );

                            Context.AddPacket(Player, new SetPartyIconOutgoingPacket(party2.Leader.Id, PartyIcon.None) );

                            Context.AddPacket(party2.Leader, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " has revoked your invitation.") );

                            Context.AddPacket(party2.Leader, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.None) );                              
                        }

                        party.AddMember(Player);

                        Context.AddPacket(Player, new SetPartyIconOutgoingPacket(Player.Id, party.SharedExperienceEnabled ? PartyIcon.YellowSharedExperience : PartyIcon.Yellow) );
                    }

                    party.AddInvitation(observer);

                    if (createParty)
                    {
                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited. Open the party channel to communicate with your members.") );
                    }
                    else
                    {
                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited.") );
                    }

                    Context.AddPacket(Player, new SetPartyIconOutgoingPacket(observer.Id, PartyIcon.WhiteBlue) );

                    Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " has invited you to " + (Player.Gender == Gender.Male ? "his" : "her") + " party." ) );

                    Context.AddPacket(observer, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.WhiteYellow) );
                }
            }

            return Promise.Completed;
        }
    }
}