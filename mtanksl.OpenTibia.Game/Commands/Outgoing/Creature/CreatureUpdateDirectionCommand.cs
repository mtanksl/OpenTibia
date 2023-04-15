using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
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
            if (Creature.Direction != Direction)
            {
                Creature.Direction = Direction;

                Tile fromTile = Creature.Tile;

                byte index = fromTile.GetIndex(Creature);

                if (index < Constants.ObjectsPerPoint)
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        if (observer == Creature)
                        {
                            PlayerActionBehaviour playerActionBehaviour = Context.Server.Components.GetComponent<PlayerActionBehaviour>(observer);

                            if (playerActionBehaviour != null)
                            {
                                Context.Server.Components.RemoveComponent(observer, playerActionBehaviour);
                            }

                            PlayerWalkBehaviour playerWalkBehaviour = Context.Server.Components.GetComponent<PlayerWalkBehaviour>(observer);

                            if (playerWalkBehaviour != null)
                            {
                                if (Context.Server.Components.RemoveComponent(observer, playerWalkBehaviour) )
                                {
                                    Context.AddPacket(observer.Client.Connection, new StopWalkOutgoingPacket(observer.Direction) );
                                }
                            }
                        }

                        if (observer.Tile.Position.CanSee(fromTile.Position) )
                        {
                            Context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(fromTile.Position, index, Creature.Id, Creature.Direction) );
                        }
                    }
                }
            }

            return Promise.Completed;
        }
    }
}