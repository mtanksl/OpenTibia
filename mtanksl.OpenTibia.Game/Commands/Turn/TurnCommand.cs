using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TurnCommand : Command
    {
        public TurnCommand(Creature creature, Direction direction)
        {
            Creature = creature;

            Direction = direction;
        }

        public Creature Creature { get; set; }

        public Direction Direction { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = Creature.Tile;

            byte fromIndex = fromTile.GetIndex(Creature);

            if (Creature.Direction != Direction)
            {
                //Act

                Creature.Direction = Direction;

                //Notify

                foreach (var observer in context.Server.Map.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(fromTile.Position, fromIndex, Creature.Id, Direction) );                        
                    }
                }
            }

            base.Execute(context);
        }
    }
}