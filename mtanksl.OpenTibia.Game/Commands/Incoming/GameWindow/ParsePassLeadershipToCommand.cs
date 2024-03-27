using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParsePassLeadershipToCommand : Command
    {
        public ParsePassLeadershipToCommand(Player player, uint creatureId)
        {
            Player = player;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public override Promise Execute()
        {
            Party party = Context.Server.Parties.GetPartyByLeader(Player);

            if (party != null)
            {
                Player observer = Context.Server.GameObjects.GetPlayer(CreatureId);

                if (observer != null && observer != Player && party.ContainsMember(observer) )
                {
                    party.Leader = observer;

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

                        Context.AddPacket(member, new SetPartyIconOutgoingPacket(Player.Id, party.SharedExperienceEnabled ? PartyIcon.BlueSharedExperience : PartyIcon.Blue) );
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

            return Promise.Completed;
        }
    }
}