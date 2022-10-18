using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateParentCommand : Command
    {
        public CreatureUpdateParentCommand(Creature creature, Tile toTile, Direction? direction = null)
        {
            Creature = creature;

            ToTile = toTile;

            Direction = direction;
        }

        public Creature Creature { get; set; }

        public Tile ToTile { get; set; }

        public Direction? Direction { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Tile fromTile = Creature.Tile;

                byte fromIndex = fromTile.GetIndex(Creature);

                fromTile.RemoveContent(fromIndex);

                byte toIndex = ToTile.AddContent(Creature);

                bool updateDirection = false;

                Direction expected = fromTile.Position.ToDirection(ToTile.Position, Creature.Direction);

                if (Direction != null && Direction.Value != expected)
                {
                    updateDirection = true;

                    expected = Direction.Value;
                }

                Creature.Direction = expected;

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer == Creature)
                    {
                        Walking(context, observer, fromTile.Position, fromIndex, ToTile.Position, toIndex, updateDirection, fromTile.Count);
                    }
                    else
                    {
                        bool canSeeFrom = observer.Tile.Position.CanSee(fromTile.Position) && fromIndex < Constants.ObjectsPerPoint;

                        bool canSeeTo = observer.Tile.Position.CanSee(ToTile.Position) && toIndex < Constants.ObjectsPerPoint;

                        if (canSeeFrom && canSeeTo)
		                {
                            context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromTile.Position, fromIndex, ToTile.Position) );

                            if (updateDirection)
                            {
                                context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(ToTile.Position, toIndex, Creature.Id, Creature.Direction) );
                            }

                            if (fromTile.Count >= Constants.ObjectsPerPoint)
                            {
                                context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(context.Server.Map, observer.Client, fromTile.Position) );
                            }
                        }
                        else if (canSeeFrom)
                        {
                            context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex) );

                            if (fromTile.Count >= Constants.ObjectsPerPoint)
                            {
                                context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(context.Server.Map, observer.Client, fromTile.Position) );
                            }
                        }
                        else if (canSeeTo)
                        {
                            uint removeId;

                            if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                            {
                                context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, toIndex, Creature) );
                            }
                            else
                            {
                                context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(ToTile.Position, toIndex, removeId, Creature) );
                            }
                        }
                    }
                }

                context.AddEvent(new TileRemoveCreatureEventArgs(fromTile, Creature, fromIndex) );

                context.AddEvent(new TileAddCreatureEventArgs(ToTile, Creature, toIndex) );

                resolve(context);
            } );
        }

        private void Walking(Context context, Player observer, Position fromPosition, byte fromIndex, Position toPosition, byte toIndex, bool updateDirection, int count)
        {
            int deltaZ = toPosition.Z - fromPosition.Z;

            int deltaY = toPosition.Y - fromPosition.Y;

            int deltaX = toPosition.X - fromPosition.X;

            if (deltaZ < -1 || deltaZ > 1 || deltaY < -6 || deltaY > 7 || deltaX < -8 || deltaX > 9 || fromIndex >= Constants.ObjectsPerPoint)
            {
                context.AddPacket(observer.Client.Connection, new SendTilesOutgoingPacket(context.Server.Map, observer.Client, ToTile.Position) );
            }
            else
            {
                if (fromPosition.Z == 7 && toPosition.Z == 8)
			    {
				    context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex) );
			    }
			    else
			    {
                    context.AddPacket(observer.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, toPosition) );

                    if (updateDirection)
                    {
                        context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(toPosition, toIndex, Creature.Id, Creature.Direction) );
                    }
                }

                Position position = fromPosition;

                while (deltaZ < 0)
                {
                    context.AddPacket(observer.Client.Connection, new SendMapUpOutgoingPacket(context.Server.Map, observer.Client, position),

                                                                  new SendMapWestOutgoingPacket(context.Server.Map, observer.Client, position.Offset(0, 1, -1) ),

                                                                  new SendMapNorthOutgoingPacket(context.Server.Map, observer.Client, position.Offset(0, 0, -1) ) );

                    position = position.Offset(0, 0, -1);

                    deltaZ++;
                }

                while (deltaZ > 0)
                {
                    context.AddPacket(observer.Client.Connection, new SendMapDownOutgoingPacket(context.Server.Map, observer.Client, position),

                                                                  new SendMapEastOutgoingPacket(context.Server.Map, observer.Client, position.Offset(0, -1, 1) ),

                                                                  new SendMapSouthOutgoingPacket(context.Server.Map, observer.Client, position.Offset(0, 0, 1) ) );

                    position = position.Offset(0, 0, 1);

                    deltaZ--;
                }

                while (deltaY < 0)
                {
                    context.AddPacket(observer.Client.Connection, new SendMapNorthOutgoingPacket(context.Server.Map, observer.Client, position.Offset(0, -1, 0) ) );

                    position = position.Offset(0, -1, 0);

                    deltaY++;
                }

                while (deltaY > 0)
                {
                    context.AddPacket(observer.Client.Connection, new SendMapSouthOutgoingPacket(context.Server.Map, observer.Client, position.Offset(0, 1, 0) ) );

                    position = position.Offset(0, 1, 0);

                    deltaY--;
                }

                while (deltaX < 0)
                {
                    context.AddPacket(observer.Client.Connection, new SendMapWestOutgoingPacket(context.Server.Map, observer.Client, position.Offset(-1, 0, 0) ) );

                    position = position.Offset(-1, 0, 0);

                    deltaX++;
                }

                while (deltaX > 0)
                {
                    context.AddPacket(observer.Client.Connection, new SendMapEastOutgoingPacket(context.Server.Map, observer.Client, position.Offset(1, 0, 0) ) );

                    position = position.Offset(1, 0, 0);

                    deltaX--;
                }

                if (fromPosition.Z == 8 && toPosition.Z == 7)
                {

                }
                else
                {
                    if (count >= Constants.ObjectsPerPoint)
                    {
                        context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(context.Server.Map, observer.Client, fromPosition) );
                    }
                }
            }
        }
    }
}