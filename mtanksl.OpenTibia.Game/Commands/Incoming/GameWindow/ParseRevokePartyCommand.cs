using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseRevokePartyCommand : Command
    {
        public ParseRevokePartyCommand(Player player, uint creatureId)
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

                if (observer != null && observer != Player && party.ContainsInvitation(observer) )
                {
                    party.RemoveInvitation(observer);

                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Invitation for " + observer.Name + " has been revoked.") );

                    Context.AddPacket(Player, new SetPartyIconOutgoingPacket(observer.Id, PartyIcon.None) );

                    Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " has revoked " + (Player.Gender == Gender.Male ? "his" : "her") + " invitation.") );

                    Context.AddPacket(observer, new SetPartyIconOutgoingPacket(Player.Id, PartyIcon.None) );
                }
            }

            return Promise.Completed;
        }
    }
}