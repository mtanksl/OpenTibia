using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateHealthCommand : Command
    {
        public CreatureUpdateHealthCommand(Creature creature, ushort health)
        {
            Creature = creature;

            Health = health;
        }

        public Creature Creature { get; set; }

        public ushort Health { get; set; }

        public override void Execute(Context context)
        {
            if (Creature.Health != Health)
            {
                Tile fromTile = Creature.Tile;

                Creature.Health = Health;

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new SetHealthOutgoingPacket(Creature.Id, (byte)Math.Ceiling(100.0 * Creature.Health / Creature.MaxHealth) ) );
                    }
                }
            }

            base.Execute(context);
        }
    }
}