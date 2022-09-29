using OpenTibia.Common.Events;
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

        public override void Execute(Context context)
        {
            if (Creature.Speed != Speed)
            {
                Tile fromTile = Creature.Tile;

                Creature.Speed = Speed;

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new SetSpeedOutgoingPacket(Creature.Id, Creature.Speed) );
                    }
                }
            }

            base.Execute(context);
        }
    }
}