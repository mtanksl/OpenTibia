using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateDirectionCommand : Command
    {
        public CreatureUpdateDirectionCommand(Creature creature, Direction direction)
        {
            Creature = creature;

            Direction = direction;
        }

        public Creature Creature { get; set; }

        public Direction Direction { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (Creature.Direction != Direction)
                {
                    Creature.Direction = Direction;

                    Tile fromTile = Creature.Tile;

                    byte index = fromTile.GetIndex(Creature);

                    if (index < Constants.ObjectsPerPoint)
                    {
                        foreach (var observer in context.Server.GameObjects.GetPlayers() )
                        {
                            if (observer == Creature)
                            {
                                if (context.Server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(observer) ) )
                                {
                                    context.AddPacket(observer.Client.Connection, new StopWalkOutgoingPacket(observer.Direction) );
                                }

                                context.Server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(observer) );
                            }

                            if (observer.Tile.Position.CanSee(fromTile.Position) )
                            {
                                context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(fromTile.Position, index, Creature.Id, Creature.Direction) );
                            }
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}