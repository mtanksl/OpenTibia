using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;

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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (Creature.PartyIcon != PartyIcon)
                {
                    Creature.PartyIcon = PartyIcon;

                    Tile fromTile = Creature.Tile;

                    foreach (var observer in context.Server.GameObjects.GetPlayers() )
                    {
                        if (observer.Tile.Position.CanSee(fromTile.Position) )
                        {
                            context.AddPacket(observer.Client.Connection, new SetPartyIconOutgoingPacket(Creature.Id, Creature.PartyIcon) );
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}