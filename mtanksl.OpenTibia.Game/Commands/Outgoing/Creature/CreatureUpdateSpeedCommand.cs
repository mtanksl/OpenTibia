using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateSpeedCommand : Command
    {
        public CreatureUpdateSpeedCommand(Creature creature, ushort speed)
        {
            Creature = creature;

            Speed = speed;
        }

        public Creature Creature { get; set; }

        public ushort Speed { get; set; }

        public override Promise Execute()
        {
            if (Creature.Speed != Speed)
            {
                Creature.Speed = Speed;

                Tile fromTile = Creature.Tile;

                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        Context.AddPacket(observer.Client.Connection, new SetSpeedOutgoingPacket(Creature.Id, Creature.Speed) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}