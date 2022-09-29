using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdatePartyIconCommand : Command
    {
        public CreatureUpdatePartyIconCommand(Creature creature, PartyIcon partyIcon)
        {
            Creature = creature;

            PartyIcon = partyIcon;
        }

        public Creature Creature { get; set; }

        public PartyIcon PartyIcon { get; set; }

        public override void Execute(Context context)
        {
            if (Creature.PartyIcon != PartyIcon)
            {
                Tile fromTile = Creature.Tile;

                Creature.PartyIcon = PartyIcon;

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new SetPartyIconOutgoingPacket(Creature.Id, Creature.PartyIcon) );
                    }
                }
            }

            base.Execute(context);
        }
    }
}