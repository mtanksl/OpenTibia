using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateParentCommand : Command
    {
        public CreatureUpdateParentCommand(Creature creature, Tile toTile, Direction? changeDirectionOnMove = null)
        {
            Creature = creature;

            ToTile = toTile;

            ChangeDirectionOnMove = changeDirectionOnMove;
        }

        public Creature Creature { get; set; }

        public Tile ToTile { get; set; }

        public Direction? ChangeDirectionOnMove { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Tile fromTile = Creature.Tile;

                byte fromIndex = fromTile.GetIndex(Creature);

                fromTile.RemoveContent(fromIndex);

                byte toIndex = ToTile.AddContent(Creature);

                bool updateDirection = false;

                Direction expected = fromTile.Position.ToDirection(ToTile.Position);

                if (expected == Direction.None)
                {
                    updateDirection = true;

                    expected = Creature.Direction;
                }

                if (ChangeDirectionOnMove != null && ChangeDirectionOnMove.Value != expected)
                {
                    updateDirection = true;

                    expected = ChangeDirectionOnMove.Value;
                }

                Creature.Direction = expected;

                foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                {
                    if (observer == Creature)
                    {
                        int deltaZ = ToTile.Position.Z - fromTile.Position.Z;

                        int deltaY = ToTile.Position.Y - fromTile.Position.Y;

                        int deltaX = ToTile.Position.X - fromTile.Position.X;

                        if (deltaZ < -1 || deltaZ > 1 || deltaY < -6 || deltaY > 7 || deltaX < -8 || deltaX > 9 || fromIndex >= Constants.ObjectsPerPoint)
                        {
                            Context.AddPacket(observer.Client.Connection, new SendTilesOutgoingPacket(Context.Server.Map, observer.Client, ToTile.Position) );
                        }
                        else
                        {
                            if (fromTile.Position.Z == 7 && ToTile.Position.Z == 8)
			                {
				                Context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex) );
			                }
			                else
			                {
                                Context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromTile.Position, fromIndex, ToTile.Position) );

                                if (updateDirection)
                                {
                                    Context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(ToTile.Position, toIndex, Creature.Id, Creature.Direction) );
                                }
                            }

                            Position position = fromTile.Position;

                            while (deltaZ < 0)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendMapUpOutgoingPacket(Context.Server.Map, observer.Client, position),

                                                                              new SendMapWestOutgoingPacket(Context.Server.Map, observer.Client, position.Offset(0, 1, -1) ),

                                                                              new SendMapNorthOutgoingPacket(Context.Server.Map, observer.Client, position.Offset(0, 0, -1) ) );

                                position = position.Offset(0, 0, -1);

                                deltaZ++;
                            }

                            while (deltaZ > 0)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendMapDownOutgoingPacket(Context.Server.Map, observer.Client, position),

                                                                              new SendMapEastOutgoingPacket(Context.Server.Map, observer.Client, position.Offset(0, -1, 1) ),

                                                                              new SendMapSouthOutgoingPacket(Context.Server.Map, observer.Client, position.Offset(0, 0, 1) ) );

                                position = position.Offset(0, 0, 1);

                                deltaZ--;
                            }

                            while (deltaY < 0)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendMapNorthOutgoingPacket(Context.Server.Map, observer.Client, position.Offset(0, -1, 0) ) );

                                position = position.Offset(0, -1, 0);

                                deltaY++;
                            }

                            while (deltaY > 0)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendMapSouthOutgoingPacket(Context.Server.Map, observer.Client, position.Offset(0, 1, 0) ) );

                                position = position.Offset(0, 1, 0);

                                deltaY--;
                            }

                            while (deltaX < 0)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendMapWestOutgoingPacket(Context.Server.Map, observer.Client, position.Offset(-1, 0, 0) ) );

                                position = position.Offset(-1, 0, 0);

                                deltaX++;
                            }

                            while (deltaX > 0)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendMapEastOutgoingPacket(Context.Server.Map, observer.Client, position.Offset(1, 0, 0) ) );

                                position = position.Offset(1, 0, 0);

                                deltaX--;
                            }

                            if (fromTile.Position.Z == 8 && ToTile.Position.Z == 7)
                            {

                            }
                            else
                            {
                                if (fromTile.Count >= Constants.ObjectsPerPoint)
                                {
                                    Context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, fromTile.Position) );
                                }
                            }
                        }

                        PlayerActionDelayBehaviour playerActionBehaviour = Context.Server.Components.GetComponent<PlayerActionDelayBehaviour>(observer);

                        if (playerActionBehaviour != null)
                        {
                            Context.Server.Components.RemoveComponent(observer, playerActionBehaviour);
                        }

                        PlayerWalkDelayBehaviour playerWalkBehaviour = Context.Server.Components.GetComponent<PlayerWalkDelayBehaviour>(observer);

                        if (playerWalkBehaviour != null)
                        {
                            if (Context.Server.Components.RemoveComponent(observer, playerWalkBehaviour) )
                            {
                                Context.AddPacket(observer.Client.Connection, new StopWalkOutgoingPacket(observer.Direction) );
                            }
                        }
                    }
                    else
                    {
                        bool canSeeFrom = observer.Tile.Position.CanSee(fromTile.Position) && fromIndex < Constants.ObjectsPerPoint;

                        bool canSeeTo = observer.Tile.Position.CanSee(ToTile.Position) && toIndex < Constants.ObjectsPerPoint;

                        if (canSeeFrom && canSeeTo)
		                {
                            Context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromTile.Position, fromIndex, ToTile.Position) );

                            if (updateDirection)
                            {
                                Context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(ToTile.Position, toIndex, Creature.Id, Creature.Direction) );
                            }

                            if (fromTile.Count >= Constants.ObjectsPerPoint)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, fromTile.Position) );
                            }
                        }
                        else if (canSeeFrom)
                        {
                            Context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex) );

                            if (fromTile.Count >= Constants.ObjectsPerPoint)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, fromTile.Position) );
                            }
                        }
                        else if (canSeeTo)
                        {
                            uint removeId;

                            if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                            {
                                Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, toIndex, Creature) );
                            }
                            else
                            {
                                Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, toIndex, removeId, Creature) );
                            }
                        }
                    }
                }

                Context.AddEvent(new TileRemoveCreatureEventArgs(fromTile, Creature, fromIndex) );

                Context.AddEvent(new TileAddCreatureEventArgs(ToTile, Creature, toIndex) );

                resolve();
            } );
        }
    }
}