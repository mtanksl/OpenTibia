using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class TileAddCreatureCommand : Command
    {
        public TileAddCreatureCommand(Tile toTile, Creature creature)
        {
            ToTile = toTile;

            Creature = creature;
        }

        public Tile ToTile { get; set; }

        public Creature Creature { get; set; }

        public override Promise Execute()
        {
            int toIndex = ToTile.AddContent(Creature);

            Context.Server.Map.ZoneAddCreature(ToTile.Position, Creature);

            Dictionary<Creature, byte> canSeeTo = new Dictionary<Creature, byte>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypeCreature(Creature.Tile.Position) )
            {
                if (observer is Player player)
                {
                    if (player != Creature)
                    {
                        byte clientIndex;

                        if (player.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            canSeeTo.Add(player, clientIndex);
                        }
                    }
                }
                else
                {
                    if (observer.Tile.Position.CanSee(Creature.Tile.Position) )
                    {
                        canSeeTo.Add(observer, 0);
                    }
                }
            }

            foreach (var pair in canSeeTo)
            {
                if (pair.Key is Player player)
                {
                    uint removeId;

                    if (player.Client.Battles.IsKnownCreature(Creature.Id, out removeId) )
                    {
                        Context.AddPacket(player, new ThingAddOutgoingPacket(ToTile.Position, pair.Value, Creature, player.Client.GetSkullIcon(Creature), player.Client.GetPartyIcon(Creature) ) );
                    }
                    else
                    {
                        Context.AddPacket(player, new ThingAddOutgoingPacket(ToTile.Position, pair.Value, removeId, Creature, player.Client.GetSkullIcon(Creature), player.Client.GetPartyIcon(Creature), player.Client.GetWarIcon(Creature) ) );
                    }
                }

                Context.AddEvent(pair.Key, ToTile.Position, new CreatureAppearEventArgs(Creature, ToTile, toIndex) );
            }

            Context.AddEvent(new TileAddCreatureEventArgs(Creature, null, null, ToTile, toIndex) );

            return Promise.Completed;
        }
    }
}